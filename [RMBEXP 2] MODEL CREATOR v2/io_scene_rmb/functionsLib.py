import bpy
from bpy.types import Object
import os	
from mathutils import *
from enum import Enum


def QuatMatrix(quat):
	return Quaternion(quat[3],quat[0],quat[1],quat[2]).toMatrix()	
	
def VectorMatrix(vector):
	return Matrix.Translation(Vector(vector))		
	
def roundVector(vec,dec=17):
	fvec=[]
	for v in vec:
		fvec.append(round(v,dec))
	return Vector(fvec)
	
def roundMatrix(mat, dec=17):
	fmat = []
	for row in mat:
		fmat.append(roundVector(row, dec))
		
	return Matrix(*fmat)

def Matrix4x4(data):
	return Matrix((data[:4], data[4:8], data[8:12], data[12:16]))

def Matrix3x3(data):
	return Matrix((data[:3], data[3:6], data[6:9]))
	
def ParseID():
	ids = []
	scene = bpy.context.scene

	for mat in bpy.data.materials:
		try:
			model_id = int(mat.name.split('-')[0])
			ids.append(model_id)
		except:pass

	for object in scene.objects:
		if object.type == 'Mesh':
			try:
				model_id = int(object.data.name.split('-')[0])
				ids.append(model_id)
			except:
				pass 

	for mesh in bpy.data.meshes:
			try:
				model_id = int(mesh.name.split('-')[0])
				ids.append(model_id)
			except:
				pass   

	try:
		model_id = max(ids)+1
	except:
		model_id = 0

	return model_id


class MeshType(Enum):
	Quad = 0
	Triangle = 1
	Mixed = 2

def check_mesh_type(obj: Object) -> MeshType:
	if not isinstance(obj, Object):
		print('ERROR: obj is not blender Object class')
		return None
	
	quads = 0
	triangles = 0

	bpy.context.view_layer.objects.active = obj
	bpy.ops.object.mode_set(mode='EDIT')

	mesh_data = bpy.context.edit_object.data

	for face in mesh_data.polygons:
		if len(face.vertices) == 4:
			quads += 1
		elif len(face.vertices) == 3:
			triangles += 1

	bpy.ops.object.mode_set(mode='OBJECT')

	type = MeshType.Quad if triangles == 0 else MeshType.Triangle
	type = MeshType.Mixed if triangles > 0 and quads > 0 else type
	return type

def split_mesh_by_faces(obj: Object):
	if not isinstance(obj, Object):
		print('ERROR: obj is not blender Object class')
		return None

	bpy.context.view_layer.objects.active = obj
	bpy.ops.object.mode_set(mode='EDIT')
	bpy.ops.mesh.edge_split(type='EDGE')
	bpy.ops.object.mode_set(mode='OBJECT')

def convert_quads_to_triangles(obj: Object):
	bpy.context.view_layer.objects.active = obj
	bpy.ops.object.mode_set(mode='EDIT')
	bpy.ops.mesh.select_all(action='SELECT')
	bpy.ops.mesh.quads_convert_to_tris(quad_method='BEAUTY', ngon_method='BEAUTY')
	bpy.ops.object.mode_set(mode='OBJECT')