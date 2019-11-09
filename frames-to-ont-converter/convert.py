import csv
import json

BASIC_ONT = '''{
    "last_id": "0",
    "namespaces": {
        "default": "http://knova.ru/user/1572791144722",
        "ontolis-avis": "http://knova.ru/ontolis-avis",
        "owl": "http://www.w3.org/2002/07/owl",
        "rdf": "http://www.w3.org/1999/02/22-rdf-syntax-ns",
        "rdfs": "http://www.w3.org/2000/01/rdf-schema",
        "xsd": "http://www.w3.org/2001/XMLSchema"
    },
    "nodes": [
    ],
    "relations": [
    ],
    "visualize_ont_path": ""
}'''


class Frame:
    def __init__(self, dict):
        id = int(dict['id'])
        name = dict['name']
        parent_id = int(dict['is_a'])

        properties_cnt = (len(dict) - 3) // 2
        properties = [(dict['property' + str(prop_num)], dict['value' + str(prop_num)])
                      for prop_num in range(1, properties_cnt + 1)]

        self.set_params(id, name, parent_id, properties)

    def set_params(self, id, name, parent_id, properties):
        self.id = id
        self.name = name
        self.parent_id = parent_id
        self.properties = properties

    def __repr__(self):
        return f'id={self.id}, name={self.name}, parent_id={self.parent_id}, properties={self.properties}'


class Ontology:
    def __init__(self, basic_json=BASIC_ONT):
        self.ont_as_dict = json.loads(basic_json)
        self.ont_id_by_real_id = {}

    def add_frame(self, frame: Frame):
        id_node = self.create_node(frame.id)
        self.add_node(id_node)
        self.ont_id_by_real_id[frame.id] = id_node['id']

        name_node = self.create_node(frame.name)
        self.add_node(name_node)

        self.add_relation('name', id_node['id'], name_node['id'])

        if frame.parent_id != -1:
            parent_ont_id = self.ont_id_by_real_id[frame.parent_id]
            self.add_relation('is_a', id_node['id'], parent_ont_id)

        props_blank_node = self.create_node('_')
        self.add_node(props_blank_node)
        self.add_relation('has', id_node['id'], props_blank_node['id'])

        for prop_order, prop in enumerate(frame.properties):
            name, value = prop

            if name.strip() == '':
                continue
            prop_node = self.create_node('_')
            self.add_node(prop_node)
            self.add_relation('#' + str(prop_order), prop_node['id'], props_blank_node['id'])

            prop_name_node = self.create_node(name)
            self.add_node(prop_name_node)
            self.add_relation('name', prop_node['id'], prop_name_node['id'])

            prop_value_node = self.create_node(value)
            self.add_node(prop_value_node)
            self.add_relation('value', prop_node['id'], prop_value_node['id'])

    def create_node(self, value):
        node = {'id': self.get_next_id(),
                'name': value,
                'namespace': '',
                "position_x": 0,
                "position_y": 0
                }
        return node

    def get_next_id(self):
        last_id = int(self.ont_as_dict['last_id'])
        self.ont_as_dict['last_id'] = str(last_id + 1)
        return last_id

    def add_node(self, node):
        self.ont_as_dict['nodes'].append(node)

    def add_relation(self, rel_name, id_from, id_to):
        relation = {
            "id": self.get_next_id(),
            "source_node_id": id_from,
            "destination_node_id": id_to,
            "name": rel_name,
            "namespace": ''
        }
        self.ont_as_dict['relations'].append(relation)


def convert(filename):
    with open(filename, 'r') as csv_file:
        reader = csv.DictReader(csv_file, delimiter=';')
        frames = [Frame(row) for row in reader]

    ontology = Ontology()
    for frame in frames:
        ontology.add_frame(frame)

    with open("cleaning.ont", "w", encoding='utf8') as ont_file:
        json.dump(ontology.ont_as_dict, ont_file, indent=4)


if __name__ == '__main__':
    convert('Выведение пятен - фреймы.csv')
