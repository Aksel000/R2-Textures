# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful, but
# WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
# General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program. If not, see <http://www.gnu.org/licenses/>.

# Original plugin author: unfortunately, I couldn't find the author's username since the website is no longer accessible.
# Plugin source: http://forum.xentax.com/viewtopic.php?f=16&t=3344 (No longer accessible)
# Modifications by: Trolll67, https://github.com/Trolll67
# Modifications: updated Python version to 3.10, rewrote Blender API to support Blender 3.6 and 4.0

bl_info = {
    "name" : "RMB format",
    "author" : "Trolll67, https://github.com/Trolll67",
    "version" : (0, 0, 1),
    "blender" : (3, 6, 0),
    "location": "File > Import-Export",
    "description" : "RMB IO meshes, UVs, textures.",
    "warning" : "",
    "doc_url": "https://github.com/Trolll67",
    "category": "Import-Export",
}

from io_scene_rmb.mesh_opt import RMBMeshToolPanel, CheckMeshTypeOperator, ConvertMeshToTrianglesOperator, SplitMeshByFacesOperator

if "bpy" in locals():
    import importlib
    if "import_rmb" in locals():
        importlib.reload(import_rmb)
    if "export_rmb" in locals():
        importlib.reload(export_rmb)
else:
    from io_scene_rmb import import_rmb
    from io_scene_rmb import export_rmb


import bpy
from bpy.props import (
        StringProperty,
        BoolProperty,
        # FloatProperty,
        EnumProperty,
        CollectionProperty,
        )
from bpy_extras.io_utils import (
        ImportHelper,
        ExportHelper,
        orientation_helper,
        axis_conversion
        )


@orientation_helper(axis_forward='-Z', axis_up='Y')
class ImportRMB(bpy.types.Operator, ImportHelper):
    """Load a RMB files"""
    bl_idname = "import_scene.rmb"
    bl_label = "Import RMB"
    bl_options = {'PRESET', 'UNDO'}

    filename_ext = ".rmb"
    filter_glob: bpy.props.StringProperty(
            default="*.rmb",
            options={'HIDDEN'},
            )
    
    files: CollectionProperty(
            name="File Path",
            type=bpy.types.OperatorFileListElement,
            )

    ui_tab: EnumProperty(
            items=(('MAIN', "Main", "Main basic settings"),
                   ('ARMATURE', "Armatures", "Armature-related settings"),
                   ),
            name="ui_tab",
            description="Import options categories",
            )
    

    def draw(self, context):
        pass
    
    def execute(self, context):
        keywords = self.as_keywords(ignore=("filter_glob", "directory", "ui_tab", "filepath", "files"))

        from . import import_rmb
        import os

        if self.files:
            ret = {'CANCELLED'}
            dirname = os.path.dirname(self.filepath)
            for file in self.files:
                path = os.path.join(dirname, file.name)
                if import_rmb.load(self, context, filepath=path, **keywords) == {'FINISHED'}:
                    ret = {'FINISHED'}
            return ret
        else:
            return import_rmb.load(self, context, filepath=self.filepath, **keywords)
        

@orientation_helper(axis_forward='-Z', axis_up='Y')
class ExportRMB(bpy.types.Operator, ExportHelper):
    """Write a RMB file"""
    bl_idname = "export_scene.rmb"
    bl_label = "Export RMB"
    bl_options = {'UNDO', 'PRESET'}

    filename_ext = ".rmb"
    filter_glob: StringProperty(default="*.rmb", options={'HIDDEN'})

    use_selection: BoolProperty(
            name="Selected Objects",
            description="Export selected and visible objects only",
            default=False,
            )
    
    use_visible: BoolProperty(
            name='Visible Objects',
            description='Export visible objects only',
            default=False
            )
    
    use_space_transform: BoolProperty(
            name="Use Space Transform",
            description="Apply global space transform to the object rotations. When disabled "
                        "only the axis space is written to the file and all object transforms are left as-is",
            default=True,
            )


    def draw(self, context):
        pass

    def execute(self, context):
        from mathutils import Matrix
        if not self.filepath:
            raise Exception("filepath not set")

        global_matrix = (axis_conversion(to_forward=self.axis_forward,
                                         to_up=self.axis_up,
                                         ).to_4x4()
                        if self.use_space_transform else Matrix())

        keywords = self.as_keywords(ignore=("check_existing",
                                            "filter_glob",
                                            "ui_tab",
                                            ))

        keywords["global_matrix"] = global_matrix

        from . import export_rmb
        return export_rmb.save(self, context, **keywords)


def menu_func_import_rmb(self, context):
    self.layout.operator(ImportRMB.bl_idname, text="RMB (.rmb)")

def menu_func_export_rmb(self, context):
    self.layout.operator(ExportRMB.bl_idname, text="RMB (.rmb)")


classes = (
    ImportRMB,
    ExportRMB,
)

def register():
    for cls in classes:
        bpy.utils.register_class(cls)

    bpy.types.TOPBAR_MT_file_import.append(menu_func_import_rmb)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export_rmb)

    #
    bpy.utils.register_class(RMBMeshToolPanel)
    bpy.utils.register_class(CheckMeshTypeOperator)
    bpy.utils.register_class(SplitMeshByFacesOperator)
    bpy.utils.register_class(ConvertMeshToTrianglesOperator)

def unregister():
    bpy.types.TOPBAR_MT_file_import.remove(menu_func_import_rmb)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export_rmb)

    for cls in classes:
        bpy.utils.unregister_class(cls)

    #
    bpy.utils.unregister_class(RMBMeshToolPanel)
    bpy.utils.unregister_class(CheckMeshTypeOperator)
    bpy.utils.unregister_class(SplitMeshByFacesOperator)
    bpy.utils.unregister_class(ConvertMeshToTrianglesOperator)

if __name__ == "__main__":
    register()