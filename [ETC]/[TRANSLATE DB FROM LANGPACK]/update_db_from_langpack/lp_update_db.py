import argparse
import csv
import pyodbc
import progress

def connect_to_database(server, database, username, password):
    try:
        conn = pyodbc.connect(
            f'DRIVER=ODBC Driver 17 for SQL Server;'
            f'SERVER={server};'
            f'DATABASE={database};'
            f'UID={username};'
            f'PWD={password};'
        )
        return conn
    except pyodbc.Error as e:
        print(f"Database connection error: {e}")
        exit(1)


    

class Data():
    def __init__(self):
        self.dt_items = {}
        self.dt_monsters = {}
        self.dt_skills = {}
        self.tp_abnormal_types = {}
        self.tp_set_item_infos = {}
        self.tp_set_effect_descs = {}
        self.dt_abnormals = {}
        self.tbl_quest_info = {}
        self.tbl_quest = {}
        self.dt_skill_tree_nodes = {}
        self.dt_skill_packs = {}
        self.tp_skill_tree = {}
        self.tbl_popup_guide_dialog = {}
        self.tbl_msg = {}
        self.tbl_dialog = {}

    def __str__(self):
        return f'Items: {len(self.dt_items)}\nMonsters: {len(self.dt_monsters)}\nSkills: {len(self.dt_skills)}\nAbnormalTypes: {len(self.tp_abnormal_types)}\n\nItemSetInfo: {len(self.tp_set_item_infos)}\nItemSetEffectDesc: {len(self.tp_set_effect_descs)}\nAbnormals: {len(self.dt_abnormals)}'

    def __repr__(self):
        return self.__str__()

    def update(self, cursor):
        if self.dt_items and self.dt_items.items():
            update_dt_items(self.dt_items, cursor)

        if self.dt_monsters and self.dt_monsters.items():
            update_dt_monsters(self.dt_monsters, cursor)
        
        if self.dt_skills and self.dt_skills.items():
            update_dt_skills(self.dt_skills, cursor)
        
        if self.tp_abnormal_types and self.tp_abnormal_types.items():
            update_tp_abnormal_types(self.tp_abnormal_types, cursor)
        
        if self.tp_set_item_infos and self.tp_set_item_infos.items():
            update_tp_set_item_infos(self.tp_set_item_infos, cursor)
        
        if self.tp_set_effect_descs and self.tp_set_effect_descs.items():
            update_tp_set_item_effect_descs(self.tp_set_effect_descs, cursor)
        
        if self.dt_abnormals and self.dt_abnormals.items():
            update_dt_abnormals(self.dt_abnormals, cursor)
        
        if self.tbl_quest_info and self.tbl_quest_info.items():
            update_tbl_quest_info(self.tbl_quest_info, cursor)
        
        if self.tbl_quest and self.tbl_quest.items():
            update_tbl_quest(self.tbl_quest, cursor)

        if self.dt_skill_tree_nodes and self.dt_skill_tree_nodes.items():
            update_dt_skill_tree_nodes(self.dt_skill_tree_nodes, cursor)
        
        if self.dt_skill_packs and self.dt_skill_packs.items():
            update_dt_skill_packs(self.dt_skill_packs, cursor)
        
        if self.tp_skill_tree and self.tp_skill_tree.items():
            update_tp_skill_tree(self.tp_skill_tree, cursor)
        
        if self.tbl_popup_guide_dialog and self.tbl_popup_guide_dialog.items():
            update_tbl_popup_guide_dialog(self.tbl_popup_guide_dialog, cursor)
        
        if self.tbl_msg and self.tbl_msg.items():
            update_tbl_msg(self.tbl_msg, cursor)
        
        if self.tbl_dialog and self.tbl_dialog.items():
            update_tbl_dialog(self.tbl_dialog, cursor)


class DT_Item():
    def __init__(self, id, name, description, fake_name, use_msg):
        self.id = id
        self.name = name
        self.description = description
        self.fake_name = fake_name
        self.use_msg = use_msg

    def __str__(self):
        return f'{self.id} {self.name} {self.description} {self.fake_name} {self.use_msg}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE DT_Item ALTER COLUMN IName nvarchar(40) COLLATE {collation} NULL")
        cursor.execute(f"ALTER TABLE DT_Item ALTER COLUMN IDesc nvarchar(250) COLLATE {collation} NULL")
        cursor.execute(f"ALTER TABLE DT_Item ALTER COLUMN IFakeName nvarchar(40) COLLATE {collation} NOT NULL")
        cursor.execute(f"ALTER TABLE DT_Item ALTER COLUMN IUseMsg nvarchar(50) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        iname = self.name.replace("'", "''")
        idesc = self.description.replace("'", "''")
        ifakename = self.fake_name.replace("'", "''")
        iusemsg = self.use_msg.replace("'", "''")

        cursor.execute(f"UPDATE DT_Item SET IName = N'{iname}', IDesc = N'{idesc}', IFakeName = N'{ifakename}', IUseMsg = N'{iusemsg}' WHERE IID = {self.id}")
        cursor.commit()

        # Check if the update was successful
        return cursor.rowcount != 0


class DT_Monster():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE DT_Monster ALTER COLUMN MName nvarchar(50) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        mname = self.name.replace("'", "''")

        cursor.execute(f"UPDATE DT_Monster SET MName = N'{mname}' WHERE MID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0

        
class DT_Skill():
    def __init__(self, id, name, description):
        self.id = id
        self.name = name
        self.description = description

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE DT_Skill ALTER COLUMN SName nvarchar(40) COLLATE {collation} NULL")
        cursor.execute(f"ALTER TABLE DT_Skill ALTER COLUMN SDesc nvarchar(70) COLLATE {collation} NULL")

    def update(self, cursor):
        sname = self.name.replace("'", "''")
        sdesc = self.description.replace("'", "''")

        cursor.execute(f"UPDATE DT_Skill SET SName = N'{sname}', SDesc = N'{sdesc}' WHERE SID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TP_AbnormalType():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TP_AbnormalType ALTER COLUMN AName nvarchar(40) COLLATE {collation} NULL")

    def update(self, cursor):
        aname = self.name.replace("'", "''")

        cursor.execute(f"UPDATE TP_AbnormalType SET AName = N'{aname}' WHERE AType = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TP_SetItemInfo():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TP_SetItemInfo ALTER COLUMN mSetName varchar(100) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        aname = self.name.replace("'", "''")

        cursor.execute(f"UPDATE TP_SetItemInfo SET mSetName = N'{aname}' WHERE mSetType = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TP_SetItemEffectDesc():
    def __init__(self, id, description):
        self.id = id
        self.description = description

    def __str__(self):
        return f'{self.id} {self.description}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TP_SetItemEffectDesc ALTER COLUMN mDesc varchar(200) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        desc = self.description.replace("'", "''")

        cursor.execute(f"UPDATE TP_SetItemEffectDesc SET mDesc = N'{desc}' WHERE mSetType = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class DT_Abnormal():
    def __init__(self, id, description):
        self.id = id
        self.description = description

    def __str__(self):
        return f'{self.id} {self.description}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        # change varchar(200) to varchar(400)
        cursor.execute(f"ALTER TABLE DT_Abnormal ALTER COLUMN ADesc varchar(400) COLLATE {collation} NULL")

    def update(self, cursor):
        adesc = self.description.replace("'", "''")

        cursor.execute(f"UPDATE DT_Abnormal SET ADesc = N'{adesc}' WHERE AID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TblQuestInfo():
    def __init__(self, id, description):
        self.id = id
        self.description = description

    def __str__(self):
        return f'{self.id} {self.description}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        # change varchar(100) to varchar(200)
        cursor.execute(f"ALTER TABLE TblQuestInfo ALTER COLUMN mDesc varchar(200) COLLATE {collation} NULL")

    def update(self, cursor):
        desc = self.description.replace("'", "''")

        cursor.execute(f"UPDATE TblQuestInfo SET mDesc = N'{desc}' WHERE mQuestNo = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TblQuest():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()

    def prepare_columns(self, cursor):
        # change nvarchar(30) to nvarchar(100)
        cursor.execute(f"ALTER TABLE TblQuest ALTER COLUMN mQuestNm nvarchar(100) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        name = self.name.replace("'", "''")

        cursor.execute(f"UPDATE TblQuest SET mQuestNm = N'{name}' WHERE mQuestNo = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class DT_SKillTreeNode():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE DT_SKillTreeNode ALTER COLUMN mName nvarchar(40) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        name = self.name.replace("'", "''")

        cursor.execute(f"UPDATE DT_SKillTreeNode SET mName = N'{name}' WHERE mSTNID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class DT_SkillPack():
    def __init__(self, id, name, description, use_msg):
        self.id = id
        self.name = name
        self.description = description
        self.use_msg = use_msg

    def __str__(self):
        return f'{self.id} {self.name} {self.description} {self.use_msg}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        # change nvarchar(40) to nvarchar(80)
        cursor.execute(f"ALTER TABLE DT_SkillPack ALTER COLUMN mName nvarchar(80) COLLATE {collation} NOT NULL")
        cursor.execute(f"ALTER TABLE DT_SkillPack ALTER COLUMN mDesc nvarchar(200) COLLATE {collation} NULL")
        # change nvarchar(50) to nvarchar(100)
        cursor.execute(f"ALTER TABLE DT_SkillPack ALTER COLUMN mUseMsg nvarchar(100) COLLATE {collation} NULL")

    def update(self, cursor):
        name = self.name.replace("'", "''")
        desc = self.description.replace("'", "''")
        use_msg = self.use_msg.replace("'", "''")

        cursor.execute(f"UPDATE DT_SkillPack SET mName = N'{name}', mDesc = N'{desc}', mUseMsg = N'{use_msg}' WHERE mSPID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TP_SKillTree():
    def __init__(self, id, name):
        self.id = id
        self.name = name

    def __str__(self):
        return f'{self.id} {self.name}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TP_SKillTree ALTER COLUMN mName nvarchar(40) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        name = self.name.replace("'", "''")

        cursor.execute(f"UPDATE TP_SKillTree SET mName = N'{name}' WHERE mSTID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TblPopupGuideDialog():
    def __init__(self, id, subject, dialog):
        self.id = id
        self.subject = subject
        self.dialog = dialog

    def __str__(self):
        return f'{self.id} {self.subject}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        # change varchar(30) to varchar(100)
        cursor.execute(f"ALTER TABLE TblPopupGuideDialog ALTER COLUMN mSubject varchar(100) COLLATE {collation} NOT NULL")
        cursor.execute(f"ALTER TABLE TblPopupGuideDialog ALTER COLUMN mDialog varchar(7000) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        subject = self.subject.replace("'", "''")
        dialog = self.dialog.replace("'", "''")

        cursor.execute(f"UPDATE TblPopupGuideDialog SET mSubject = N'{subject}', mDialog = N'{dialog}' WHERE mGuideNo = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TblMsg():
    def __init__(self, id, description):
        self.id = id
        self.description = description

    def __str__(self):
        return f'{self.id} {self.description}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TblMsg ALTER COLUMN mDesc varchar(500) COLLATE {collation} NOT NULL")

    def update(self, cursor):
        desc = self.description.replace("'", "''")

        cursor.execute(f"UPDATE TblMsg SET mDesc = N'{desc}' WHERE mHashID = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0


class TblDialog():
    def __init__(self, id, click):
        self.id = id
        self.click = click

    def __str__(self):
        return f'{self.id} {self.click}'

    def __repr__(self):
        return self.__str__()
    
    def prepare_columns(self, cursor):
        cursor.execute(f"ALTER TABLE TblDialog ALTER COLUMN mClick varchar(8000) COLLATE {collation} NULL")

    def update(self, cursor):
        click = self.click.replace("'", "''")

        cursor.execute(f"UPDATE TblDialog SET mClick = N'{click}' WHERE mMId = {self.id}")
        cursor.commit()

        return cursor.rowcount != 0




def parse_tsv(filepath, models):
    with open(filepath, 'r', newline='', encoding="utf-16") as file:
        rd = csv.reader(file, delimiter="\t", quotechar='"') # lineterminator='\r\n'

        dt_items = {}
        dt_monsters = {}
        dt_skills = {}
        tp_abnormal_types = {}
        tp_set_item_infos = {}
        tp_set_effect_descs = {}
        dt_abnormals = {}
        tbl_quest_info = {}
        tbl_quest = {}
        dt_skill_tree_nodes = {}
        dt_skill_packs = {}
        tp_skill_tree = {}
        tbl_popup_guide_dialog = {}
        tbl_msg = {}
        tbl_dialog = {}

        is_parse_items = models is None or 'DT_Item' in models
        is_parse_monsters = models is None or 'DT_Monster' in models
        is_parse_skills = models is None or 'DT_Skill' in models
        is_parse_abnormal_types = models is None or 'TP_AbnormalType' in models
        is_parse_set_item_infos = models is None or 'TP_SetItemInfo' in models
        is_parse_set_effect_descs = models is None or 'TP_SetItemEffectDesc' in models
        is_parse_abnormals = models is None or 'DT_Abnormal' in models
        is_parse_quest_info = models is None or 'TblQuestInfo' in models
        is_parse_quest = models is None or 'TblQuest' in models
        is_parse_skill_tree_nodes = models is None or 'DT_SKillTreeNode' in models
        is_parse_skill_packs = models is None or 'DT_SkillPack' in models
        is_parse_skill_tree = models is None or 'TP_SKillTree' in models
        is_parse_popup_guide_dialog = models is None or 'TblPopupGuideDialog' in models
        is_parse_msg = models is None or 'TblMsg' in models
        is_parse_dialog = models is None or 'TblDialog' in models

        for row in rd:
            # validate row
            if len(row) != 5:
                continue

            # parse DT_Item
            if row[1] == '1' or row[1] == '2' or row[1] == '4' or row[1] == '5':
                if is_parse_items:
                    parse_dt_item(dt_items, row)

            # parse DT_Monster
            elif row[1] == '6':
                if is_parse_monsters:
                    parse_dt_monster(dt_monsters, row)

            # parse DT_Skill
            elif row[1] == '9' or row[1] == '10':
                if is_parse_skills:
                    parse_dt_skill(dt_skills, row)

            # parse TP_AbnormalType
            elif row[1] == '26':
                if is_parse_abnormal_types:
                    parse_tp_abnormal_type(tp_abnormal_types, row)

            # parse TP_SetItemInfo
            elif row[1] == '29':
                if is_parse_set_item_infos:
                    parse_tp_set_item_info(tp_set_item_infos, row)

            # parse TP_SetItemEffectDesc
            elif row[1] == '30':
                if is_parse_set_effect_descs:
                    parse_tp_set_item_effect_desc(tp_set_effect_descs, row)

            # parse DT_Abnormal
            elif row[1] == '32':
                if is_parse_abnormals:
                    parse_dt_abnormal(dt_abnormals, row)

            # parse TblQuestInfo
            elif row[1] == '33':
                if is_parse_quest_info:
                    parse_tbl_quest_info(tbl_quest_info, row)

            # parse TblQuest
            elif row[1] == '35':
                if is_parse_quest:
                    parse_tbl_quest(tbl_quest, row)

            # parse DT_SKillTreeNode
            elif row[1] == '36':
                if is_parse_skill_tree_nodes:
                    parse_dt_skill_tree_node(dt_skill_tree_nodes, row)
                
            # parse DT_SkillPack
            elif row[1] == '37' or row[1] == '38' or row[1] == '39':
                if is_parse_skill_packs:
                    parse_dt_skill_pack(dt_skill_packs, row)

            # parse TP_SKillTree
            elif row[1] == '40':
                if is_parse_skill_tree:
                    parse_tp_skill_tree(tp_skill_tree, row)

            # parse TblPopupGuideDialog
            elif row[1] == '42' or row[1] == '43':
                if is_parse_popup_guide_dialog:
                    parse_tbl_popup_guide_dialog(tbl_popup_guide_dialog, row)

            # parse TblMsg
            elif row[1] == '10001' or row[1] == '10002' or row[1] == '10003' or row[1] == '10004' or row[1] == '10005':
                if is_parse_msg:
                    parse_tbl_msg(tbl_msg, row)

            # parse TblDialog
            elif row[1] == '9999':
                if is_parse_dialog:
                    parse_tbl_dialog(tbl_dialog, row)

            # print(f'Parse: {row}')

        data = Data()
        data.dt_items = dt_items
        data.dt_monsters = dt_monsters
        data.dt_skills = dt_skills
        data.tp_abnormal_types = tp_abnormal_types
        data.tp_set_item_infos = tp_set_item_infos
        data.tp_set_effect_descs = tp_set_effect_descs
        data.dt_abnormals = dt_abnormals
        data.tbl_quest_info = tbl_quest_info
        data.tbl_quest = tbl_quest
        data.dt_skill_tree_nodes = dt_skill_tree_nodes
        data.dt_skill_packs = dt_skill_packs
        data.tp_skill_tree = tp_skill_tree
        data.tbl_popup_guide_dialog = tbl_popup_guide_dialog
        data.tbl_msg = tbl_msg
        data.tbl_dialog = tbl_dialog

        return data


def parse_dt_item(data, row):
    item = data.get(row[2])
    if item is None:
        if row[1] == '1':
            item = DT_Item(row[2], '', row[4], '', '')
        elif row[1] == '2':
            item = DT_Item(row[2], row[4], '', '', '')
        elif row[1] == '4':
            item = DT_Item(row[2], '', '', row[4], '')
        elif row[1] == '5':
            item = DT_Item(row[2], '', '', '', row[4])

        if item is not None:
            data[row[2]] = item
    else:
        if row[1] == '1':
            item.description = row[4]
        elif row[1] == '2':
            item.name = row[4]
        elif row[1] == '4':
            item.fake_name = row[4]
        elif row[1] == '5':
            item.use_msg = row[4]


def parse_dt_monster(data, row):
    item = data.get(row[2])
    if item is None:
        item = DT_Monster(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_dt_skill(data, row):
    item = data.get(row[2])
    if item is None:
        if row[1] == '9':
            item = DT_Skill(row[2], row[4], '')
        elif row[1] == '10':
            item = DT_Skill(row[2], '', row[4])

        if item is not None:
            data[row[2]] = item
    else:
        if row[1] == '9':
            item.name = row[4]
        elif row[1] == '10':
            item.description = row[4]


def parse_tp_abnormal_type(data, row):
    item = data.get(row[2])
    if item is None:
        item = TP_AbnormalType(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_dt_abnormal(data, row):
    item = data.get(row[2])
    if item is None:
        item = DT_Abnormal(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.description = row[4]


def parse_tp_set_item_info(data, row):
    item = data.get(row[2])
    if item is None:
        item = TP_SetItemInfo(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_tp_set_item_effect_desc(data, row):
    item = data.get(row[2])
    if item is None:
        item = TP_SetItemEffectDesc(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.description = row[4]


def parse_tbl_quest_info(data, row):
    item = data.get(row[2])
    if item is None:
        item = TblQuesrInfo(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.description = row[4]


def parse_tbl_quest(data, row):
    item = data.get(row[2])
    if item is None:
        item = TblQuest(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_dt_skill_tree_node(data, row):
    item = data.get(row[2])
    if item is None:
        item = DT_SKillTreeNode(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_dt_skill_pack(data, row):
    item = data.get(row[2])
    if item is None:
        if row[1] == '37':
            item = DT_SkillPack(row[2], row[4], '', '')
        elif row[1] == '38':
            item = DT_SkillPack(row[2], '', row[4], '')
        elif row[1] == '39':
            item = DT_SkillPack(row[2], '', '', row[4])

        if item is not None:
            data[row[2]] = item
    else:
        if row[1] == '37':
            item.name = row[4]
        elif row[1] == '38':
            item.description = row[4]
        elif row[1] == '39':
            item.use_msg = row[4]


def parse_tp_skill_tree(data, row):
    item = data.get(row[2])
    if item is None:
        item = TP_SKillTree(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.name = row[4]


def parse_tbl_popup_guide_dialog(data, row):
    item = data.get(row[2])
    if item is None:
        if row[1] == '42':
            item = TblPopupGuideDialog(row[2], row[4], '')
        elif row[1] == '43':
            item = TblPopupGuideDialog(row[2], '', row[4])

        if item is not None:
            data[row[2]] = item
    else:
        if row[1] == '42':
            item.subject = row[4]
        elif row[1] == '43':
            item.dialog = row[4]


def parse_tbl_msg(data, row):
    item = data.get(row[2])
    if item is None:
        item = TblMsg(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.description = row[4]


def parse_tbl_dialog(data, row):
    item = data.get(row[2])
    if item is None:
        item = TblDialog(row[2], row[4])

        if item is not None:
            data[row[2]] = item
    else:
        item.click = row[4]




def update_dt_items(dt_items, cursor):
    count = len(dt_items)
    print(f'\nUpdating {count} DT_Items...')

    idx = 0
    # results = {}
    # prepare columns
    if dt_items and dt_items.items():
        # get first element
        first_item = next(iter(dt_items.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    for id, item in dt_items.items():
        # result = item.update(cursor)
        # results[id] = result
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1

    # for item_id in results:
    #     result = results[item_id]
    #     if not result:
    #         print(f'Failed to update item {item_id}')


def update_dt_skills(dt_skills, cursor):
    count = len(dt_skills)
    print(f'\n\nUpdating {count} DT_Skills...')

    # prepare columns
    if dt_skills and dt_skills.items():
        # get first element
        first_item = next(iter(dt_skills.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in dt_skills.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_dt_monsters(dt_monsters, cursor):
    count = len(dt_monsters)
    print(f'\n\nUpdating {count} DT_Monsters...')

    # prepare columns
    if dt_monsters and dt_monsters.items():
        # get first element
        first_item = next(iter(dt_monsters.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in dt_monsters.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_dt_abnormals(dt_abnormals, cursor):
    count = len(dt_abnormals)
    print(f'\n\nUpdating {count} DT_Abnormals...')

    # prepare columns
    if dt_abnormals and dt_abnormals.items():
        # get first element
        first_item = next(iter(dt_abnormals.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in dt_abnormals.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tp_abnormal_types(tp_abnormal_types, cursor):
    count = len(tp_abnormal_types)
    print(f'\n\nUpdating {count} TP_AbnormalTypes...')

    # prepare columns
    if tp_abnormal_types and tp_abnormal_types.items():
        # get first element
        first_item = next(iter(tp_abnormal_types.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tp_abnormal_types.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tp_set_item_infos(tp_set_item_infos, cursor):
    count = len(tp_set_item_infos)
    print(f'\n\nUpdating {count} TP_SetItemInfos...')

    # prepare columns
    if tp_set_item_infos and tp_set_item_infos.items():
        # get first element
        first_item = next(iter(tp_set_item_infos.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tp_set_item_infos.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tp_set_item_effect_descs(tp_set_effect_descs, cursor):
    count = len(tp_set_effect_descs)
    print(f'\n\nUpdating {count} TP_SetItemEffectDescs...')

    # prepare columns
    if tp_set_effect_descs and tp_set_effect_descs.items():
        # get first element
        first_item = next(iter(tp_set_effect_descs.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tp_set_effect_descs.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tbl_quest_info(tbl_quest_info, cursor):
    count = len(tbl_quest_info)
    print(f'\n\nUpdating {count} TblQuestInfo...')

    # prepare columns
    if tbl_quest_info and tbl_quest_info.items():
        # get first element
        first_item = next(iter(tbl_quest_info.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tbl_quest_info.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tbl_quest(tbl_quest, cursor):
    count = len(tbl_quest)
    print(f'\n\nUpdating {count} TblQuest...')

    # prepare columns
    if tbl_quest and tbl_quest.items():
        # get first element
        first_item = next(iter(tbl_quest.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tbl_quest.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_dt_skill_tree_nodes(dt_skill_tree_nodes, cursor):
    count = len(dt_skill_tree_nodes)
    print(f'\n\nUpdating {count} DT_SKillTreeNode...')

    # prepare columns
    if dt_skill_tree_nodes and dt_skill_tree_nodes.items():
        # get first element
        first_item = next(iter(dt_skill_tree_nodes.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in dt_skill_tree_nodes.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_dt_skill_packs(dt_skill_packs, cursor):
    count = len(dt_skill_packs)
    print(f'\n\nUpdating {count} DT_SkillPack...')

    # prepare columns
    if dt_skill_packs and dt_skill_packs.items():
        # get first element
        first_item = next(iter(dt_skill_packs.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in dt_skill_packs.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tp_skill_tree(tp_skill_trees, cursor):
    count = len(tp_skill_trees)
    print(f'\n\nUpdating {count} TP_SKillTree...')

    # prepare columns
    if tp_skill_trees and tp_skill_trees.items():
        # get first element
        first_item = next(iter(tp_skill_trees.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tp_skill_trees.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tbl_popup_guide_dialog(tbl_popup_guide_dialog, cursor):
    count = len(tbl_popup_guide_dialog)
    print(f'\n\nUpdating {count} TblPopupGuideDialog...')

    # prepare columns
    if tbl_popup_guide_dialog and tbl_popup_guide_dialog.items():
        # get first element
        first_item = next(iter(tbl_popup_guide_dialog.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tbl_popup_guide_dialog.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tbl_msg(tbl_msg, cursor):
    count = len(tbl_msg)
    print(f'\n\nUpdating {count} TblMsg...')

    # prepare columns
    if tbl_msg and tbl_msg.items():
        # get first element
        first_item = next(iter(tbl_msg.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tbl_msg.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1


def update_tbl_dialog(tbl_dialog, cursor):
    count = len(tbl_dialog)
    print(f'\n\nUpdating {count} TblDialog...')

    # prepare columns
    if tbl_dialog and tbl_dialog.items():
        # get first element
        first_item = next(iter(tbl_dialog.items()))
        # check if first element is not null
        if first_item:
            # prepare columns
            first_item[1].prepare_columns(cursor)

    idx = 0
    for id, item in tbl_dialog.items():
        item.update(cursor)

        progress.bar(idx + 1, count)
        idx += 1




# parse arguments
parser = argparse.ArgumentParser(description='Generating LangPack and update Database based on LangPack tool.')
parser.add_argument('file', help='TSV file name')
# get argument to specify which model to update
parser.add_argument('-m', '--model', help='Model to update', choices=['DT_Item', 'DT_Monster', 'DT_Skill', 'TP_AbnormalType', 'TP_SetItemInfo', 'TP_SetItemEffectDesc', 'DT_Abnormal', 'TblQuestInfo', 'TblQuest', 'DT_SKillTreeNode', 'DT_SkillPack', 'TP_SKillTree', 'TblPopupGuideDialog', 'TblMsg', 'TblDialog'])
# or update all models
parser.add_argument('-a', '--all', help='Update all models', action='store_true')

args = parser.parse_args()

collation = 'Cyrillic_General_CI_AS'

# run
try:
    # prepare the columns for the specified model
    if args.all:
        models = ['DT_Item', 'DT_Monster', 'DT_Skill', 'TP_AbnormalType', 'TP_SetItemInfo', 'TP_SetItemEffectDesc', 'DT_Abnormal', 'TblQuestInfo', 'TblQuest', 'DT_SKillTreeNode', 'DT_SkillPack', 'TP_SKillTree', 'TblPopupGuideDialog', 'TblMsg', 'TblDialog']
    else:
        models = [args.model]

    data = parse_tsv(args.file, models)
    
    conn = connect_to_database('localhost,1434', 'FNLParm', 'sa', 'test@test1')
    cursor = conn.cursor()

    # update all models
    data.update(cursor)

finally:
    cursor.close()
    conn.close()