import struct
import os

	
class BinaryReader():
	"""general BinaryReader class"""
	def __init__(self, inputFile):
		self.inputFile = inputFile
		self.endian = '<'
		self.debug = False
		self.dirname = os.path.dirname(self.inputFile.name)
		self.basename = os.path.basename(self.inputFile.name).split('.')[0]
		self.ext = os.path.basename(self.inputFile.name).split('.')[-1]
		self.xorKey = None
		self.xorOffset = 0
		self.xorData = ''
		
	def XOR(self, data):
			self.xorData = ''
			
			for m in range(len(data)):
				ch = ord(chr(data[m] ^ self.xorKey[self.xorOffset]))
				self.xorData += struct.pack('B', ch)
				
				if self.xorOffset == len(self.xorKey)-1:
					self.xorOffset = 0
				else:
					self.xorOffset += 1	
				
	def dirname(self):
		return os.path.dirname(self.inputFile.name)
	def basename(self):
		return os.path.basename(self.inputFile.name).split('.')[0]
	def ext(self):
		return os.path.basename(self.inputFile.name).split('.')[-1]
		
	def print_log(self, message):
		if self.debug == True:
			print(message)

	def to_hex(self, data):
		return ' '.join(hex(x) for x in data)

	def fileSize(self):
		back = self.inputFile.tell()
		self.inputFile.seek(0, 2)
		tell = self.inputFile.tell()
		self.inputFile.seek(back)
		return tell
			
	def tell(self):
		val = self.inputFile.tell()
		self.print_log(f'current offset is {val}')

		return val	

	def read_bool(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(1)
		data = struct.unpack('?', data)[0]

		self.print_log(f'read bool: {data}')
		return data
	
	def read_int8(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(1)
		data = struct.unpack(self.endian+'b', data)[0]

		self.print_log(f'read int8: {data}')
		return data
	
	def read_uint8(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(1)
		data = struct.unpack(self.endian+'B', data)[0]

		self.print_log(f'read uint8: {data}')
		return data
	
	def read_int16(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(2)
		data = struct.unpack(self.endian+'h', data)[0]

		self.print_log(f'read int16: {data}')
		return data
	
	def read_uint16(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(2)
		data = struct.unpack(self.endian+'H', data)[0]

		self.print_log(f'read uint16: {data}')
		return data

	def read_int32(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(4)
		data = struct.unpack(self.endian+'i', data)[0]

		self.print_log(f'read int32: {data}')
		return data
	
	def read_uint32(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(4)
		data = struct.unpack(self.endian+'I', data)[0]

		self.print_log(f'read uint32: {data}')
		return data
	
	def read_int64(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(8)
		data = struct.unpack(self.endian+'q', data)[0]

		self.print_log(f'read int64: {data}')
		return data

	def read_uint64(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(8)
		data = struct.unpack(self.endian+'Q', data)[0]

		self.print_log(f'read uint64: {data}')
		return data
	
	def read_float32(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(4)
		data = struct.unpack(self.endian+'f', data)[0]

		self.print_log(f'read float32: {data}')
		return data
	
	def read_float64(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(8)
		data = struct.unpack(self.endian+'d', data)[0]

		self.print_log(f'read float64: {data}')
		return data
	
	def read_bytes(self, count):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(count)

		self.print_log(f'read bytes: {data}')
		return data
	
	def read_string(self, limit=1000):
		if self.inputFile.mode != 'rb':
			return
		
		completed = False
		data = ''
		for i in range(limit):
			char = struct.unpack('c', self.inputFile.read(1))[0]
			if char == b'\x00' or completed == True:
				completed = True
				continue # not break to set cursor to right position

			data += char.decode('utf-8', errors='ignore')

		self.print_log(f'read string: {data}')
		return data

	def read_matrix4x4(self):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(16*4)
		data = struct.unpack(self.endian+16*'f', data)

		self.print_log(f'read matrix4x4: {data}')
		return data

	def read_unknown(self, count):
		if self.inputFile.mode != 'rb':
			return
		
		data = self.inputFile.read(count)

		self.print_log(f'read unknown {count} bytes')
		return data
		
	def write_bool(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, bool)
		self.print_log(f'write bool: {data}')
		data = struct.pack('?', data)
		self.inputFile.write(data)

	def write_int8(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write int8: {data}')
		data = struct.pack(self.endian+'b', data)
		self.inputFile.write(data)

	def write_uint8(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write uint8: {data}')
		data = struct.pack(self.endian+'B', data)
		self.inputFile.write(data)

	def write_int16(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write int16: {data}')
		data = struct.pack(self.endian+'h', data)
		self.inputFile.write(data)

	def write_uint16(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write uint16: {data}')
		data = struct.pack(self.endian+'H', data)
		self.inputFile.write(data)

	def write_int32(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write int32: {data}')
		data = struct.pack(self.endian+'i', data)
		self.inputFile.write(data)

	def write_uint32(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write uint32: {data}')
		data = struct.pack(self.endian+'I', data)
		self.inputFile.write(data)

	def write_int64(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write int64: {data}')
		data = struct.pack(self.endian+'q', data)
		self.inputFile.write(data)
	
	def write_uint64(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, int)
		self.print_log(f'write uint64: {data}')
		data = struct.pack(self.endian+'Q', data)
		self.inputFile.write(data)

	def write_float32(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, float)
		self.print_log(f'write float32: {data}')
		data = struct.pack(self.endian+'f', data)
		self.inputFile.write(data)

	def write_float64(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, float)
		self.print_log(f'write float64: {data}')
		data = struct.pack(self.endian+'d', data)
		self.inputFile.write(data)

	def write_bytes(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, bytes)
		self.print_log(f'write bytes: {data}')
		self.inputFile.write(data)

	def write_string(self, data):
		if self.inputFile.mode != 'wb':
			return
		
		assert isinstance(data, str)
		self.print_log(f'write string: {data}')
		data = data.encode('utf-8')
		self.inputFile.write(data)

	def write_unknown(self, count):
		if self.inputFile.mode != 'wb':
			return
		
		self.print_log(f'write unknown {count} bytes')
		data = b'\x00' * count
		self.inputFile.write(data)