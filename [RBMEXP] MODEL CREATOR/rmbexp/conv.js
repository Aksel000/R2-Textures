// rmb exporter,importer

const Version = '1.0.1';
const fs = require('fs');
const util = require('util');


function Float3 ( x, y, z ) {
	this.x = x;
	this.y = y;
	this.z = z;
};


Float3.prototype = {
	constructor: Float3,

	clone: function() {
		return new Float3(this.x, this.y, this.z);
	},

	normalize: function() {
		/* compute magnitude of the quaternion */
		var mag = Math.sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		/* check for bogus length, to protect against divide by zero */
		if (mag > 0.0) {
			/* normalize it */
			const oneOverMag = 1.0 / mag;
			this.x *= oneOverMag;
			this.y *= oneOverMag;
			this.z *= oneOverMag;
		}
		return this;
	},

	Length: function() {
		return Math.sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
	},

	Umn: function(v) {
		this.x = this.x * v;
		this.y = this.y * v;
		this.z = this.z * v;
		return this;
	},

	Div: function(v) {
		this.x = this.x / v;
		this.y = this.y / v;
		this.z = this.z / v;
		return this;
	},

	Plus: function(v) {
		this.x = this.x + v.x;
		this.y = this.y + v.y;
		this.z = this.z + v.z;
		return this;
	},

	Minus: function(v) {
		this.x = this.x - v.x;
		this.y = this.y - v.y;
		this.z = this.z - v.z;
		return this;
	},

	crossProduct: function(v) {
		var out = new Float3(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x)
		return out;
	},

	dotProduct: function(v) {
		var out = parseFloat(0);
		out += (this.x * v.x);
		out += (this.y * v.y);
		out += (this.z * v.z);
		return out;
	},

	prn: function(g=0) {
		if (g==0) return this.x + ':' + this.y + ':' + this.z;
		if (g==1) return this.x.toFixed(6) + ':' + this.y.toFixed(6) + ':' + this.z.toFixed(6);
		if (g==2){
			return '';
		}
	}

}

function DataReader ( buffer ) {
	this.dv = new DataView( buffer );
	this.offset = 0;
	this.littleEndian = true;
};

DataReader.prototype = {
	constructor: DataReader,

	getInt8: function () {
		var value = this.dv.getInt8( this.offset );
		this.offset += 1;
		return value;
	},
	getInt8Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getInt8() );
		}
		return a;
	},

	getUint8: function () {
		var value = this.dv.getUint8( this.offset );
		this.offset += 1;
		return value;
	},
	getUint8Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getUint8() );
		}
		return a;
	},


	getInt16: function () {
		var value = this.dv.getInt16( this.offset, this.littleEndian );
		this.offset += 2;
		return value;
	},
	getInt16Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getInt16() );
		}
		return a;
	},

	getUint16: function () {
		var value = this.dv.getUint16( this.offset, this.littleEndian );
		this.offset += 2;
		return value;
	},
	getUint16Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getUint16() );
		}
		return a;
	},

	getInt32: function () {
		var value = this.dv.getInt32( this.offset, this.littleEndian );
		this.offset += 4;
		return value;
	},
	getInt32Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getInt32() );
		}
		return a;
	},

	getUint32: function () {
		var value = this.dv.getUint32( this.offset, this.littleEndian );
		this.offset += 4;
		return value;
	},
	getUint32Array: function ( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getUint32() );
		}
		return a;
	},

	getFloat32: function () {
		var value = this.dv.getFloat32( this.offset, this.littleEndian );
		this.offset += 4;
		return value;
	},
	getFloat32Array: function( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getFloat32() );
		}
		return a;
	},

	getFloat64: function () {
		var value = this.dv.getFloat64( this.offset, this.littleEndian );
		this.offset += 8;
		return value;
	},
	getFloat64Array: function( size ) {
		var a = [];
		for ( var i = 0; i < size; i++ ) {
			a.push( this.getFloat64() );
		}
		return a;
	},


	getChars: function ( size ) {
		var str = '';
		while ( size > 0 ) {
			var value = this.getUint8();
			size--;
			if ( value === 0 ) break;
			str += String.fromCharCode( value );
		}
		while ( size > 0 ) {
			this.getUint8();
			size--;
		}
		return str;
	},

	getUnicodeStrings: function ( size ) {
		var str = '';
		while ( size > 0 ) {
			var value = this.getUint16();
			size -= 2;
			if ( value === 0 ) break;
			str += String.fromCharCode( value );
		}
		while ( size > 0 ) {
			this.getUint8();
			size--;
		}
		return str;
	},

	getTextBuffer: function () {
		var size = this.getUint32();
		return this.getUnicodeStrings( size );
	},

	setPos: function(a) {
		this.offset = a;
	},
	getPos: function() {
		return this.offset;
	},

};
function BinaryWriter () {
	this.buf = Buffer.alloc(0);
	this.offset = 0;
};
BinaryWriter.prototype = {

	constructor: BinaryWriter,

	writeInt8: function (v) {
		var tmp = Buffer.allocUnsafe(1);
		tmp.writeInt8(v);
		this.writeBuf(tmp);
	},
	writeInt32: function (v) {
		var tmp = Buffer.allocUnsafe(4);
		tmp.writeInt32LE(v)
		this.writeBuf(tmp);
	},
	writeInt16: function (v) {
		var tmp = Buffer.allocUnsafe(2);
		tmp.writeInt16LE(v)
		this.writeBuf(tmp);
	},
	writeFloat: function (v) {
		var tmp = Buffer.allocUnsafe(4);
		tmp.writeFloatLE(v);
		this.writeBuf(tmp);
	},

	writeBuf: function(buf) {
		this.buf = Buffer.concat([this.buf, buf]);
		this.offset += buf.length;
	},
	writeBin: function(bin) {
		this.writeBuf(Buffer.from(bin, 'binary'));
	},

	writeStrLen: function(bin, len, ch) {
		var buf = Buffer.alloc(len, ch);
		buf.write(bin, 0);
		this.writeBuf(buf);
	},

	getPosition: function() {
		return this.offset;
	},

}



function RmbClass() {
	this.dv;
	this.filename = '';
	this.itemFlag = 0;
	this.unknown;
	this.textureCount = 0;
	this.meshCount = 0;
	this.boneCount = 0;
	this.dataOffset = 0;
	this.texturesName = [];
	this.meshes = [];
	this.meshdata = [];
};
RmbClass.prototype.parseFile = function ( filename ) {
	try {
		if (! fs.statSync(filename) ) throw new Error('No file: ' + filename);
	} catch (ex) {
		throw new Error('No file: ' + filename)
	}
	this.filename = filename;
	slog('Reading file: ' + filename);

	try {
		var buffer = fs.readFileSync(filename);

		this.dv = new DataReader( buffer.buffer );

		this.itemFlag = this.dv.getUint32();
		this.unknown = this.dv.getUint32Array(4);
		this.textureCount = this.dv.getUint32();
		this.meshCount = this.dv.getUint32();
		this.boneCount = this.dv.getUint32();
		this.dataOffset = this.dv.getUint32();
		for (let i=0; i<this.textureCount; i++) {
			this.texturesName[i] = this.dv.getChars(260);
		}
		for (let i=0; i<this.meshCount; i++) {
			this.meshes[i] = this.readMeshHead(this.dv);
		}
		this.dv.setPos(this.dataOffset);
		for (let i=0; i<this.meshCount; i++) {
			this.meshdata[i] = this.readMeshData(this.dv, this.meshes[i]);
		}
	} catch (ex) {
		throw new Error('Erorr structure');
	}
}
RmbClass.prototype.serialize = function () {
	var dv = new BinaryWriter();
	dv.writeInt32(this.itemFlag);
	this.unknown.forEach(function(v){
		dv.writeInt32(v);
	});
	dv.writeInt32(this.textureCount);
	dv.writeInt32(this.meshCount);
	dv.writeInt32(this.boneCount);
	dv.writeInt32(this.dataOffset);
	for (let i=0; i<this.textureCount; i++) {
		dv.writeStrLen(this.texturesName[i] + String.fromCharCode(0), 260, ' ');
	}
	for (let i=0; i<this.meshCount; i++) {
		this.writeMeshHead(dv, this.meshes[i]);
	}
	for (let i=0; i<this.meshCount; i++) {
		this.writeMeshData(dv, this.meshes[i], this.meshdata[i]);
	}
	return dv.buf;
}
RmbClass.prototype.readMeshHead = function ( dv ) {
	var mesh = {
		index: dv.getUint32(),
		unknown1: dv.getUint32(),
		name: dv.getChars(64),
		parent: dv.getChars(64),
		extraFlag: dv.getUint32(),
		unknown2: dv.getUint32(),
		unknown3: dv.getUint32(),
		vertexCount: dv.getUint32(),
		indexCount: dv.getUint32(),
		u4: dv.getUint8Array(1200),
		u5count: dv.getUint32(),
		u5: dv.getUint8Array(400-4),
		u6count: dv.getUint32(),
		u6: dv.getUint8Array(400-4),
	}
	return mesh;
}
RmbClass.prototype.writeMeshHead = function ( dv, mesh ) {
	mesh.u5count = mesh.indexCount;
	mesh.u6count = mesh.vertexCount;

	dv.writeInt32(mesh.index);
	dv.writeInt32(mesh.unknown1);
	dv.writeStrLen(mesh.name + String.fromCharCode(0), 64, ' ');
	dv.writeStrLen(mesh.parent + String.fromCharCode(0), 64, ' ');
	dv.writeInt32(mesh.extraFlag);
	dv.writeInt32(mesh.unknown2);
	dv.writeInt32(mesh.unknown3);
	dv.writeInt32(mesh.vertexCount);
	dv.writeInt32(mesh.indexCount);
	dv.writeBuf(Buffer.from(mesh.u4));
	dv.writeInt32(mesh.u5count);
	dv.writeBuf(Buffer.from(mesh.u5));
	dv.writeInt32(mesh.u6count);
	dv.writeBuf(Buffer.from(mesh.u6));
}
RmbClass.prototype.readMeshData = function ( dv, mesh ) {
	var vert = [], ind = [];
	for (let i=0; i<mesh.vertexCount; i++) {
		vert[i] = {pos:[], nm:[], uv:[], w1:[], w2:[], bw:[], bi:[]};
		vert[i].pos = dv.getFloat32Array(3);
	}
	for (let i=0; i<mesh.vertexCount; i++) {
		vert[i].nm = dv.getFloat32Array(3);
	}
	for (let i=0; i<mesh.vertexCount; i++) {
		vert[i].uv = dv.getFloat32Array(2);
	}
	for (let i=0; i<mesh.vertexCount; i++) {
		vert[i].w1 = dv.getFloat32Array(3);
	}
	for (let i=0; i<mesh.vertexCount; i++) {
		vert[i].w2 = dv.getFloat32Array(3);
	}
	for (let i=0; i<mesh.indexCount; i++) {
		ind[i] = dv.getUint16();
	}
	if (mesh.extraFlag == 1) {
		for (let i=0; i<mesh.indexCount; i++) {
			vert[i].bw = dv.getFloat32Array(4);
		}
		for (let i=0; i<mesh.indexCount; i++) {
			vert[i].bi = dv.getUint32Array(4);
		}
	}
	return {v: vert, f: ind};
}
RmbClass.prototype.writeMeshData = function ( dv, meshInfo, data ) {
	for (let i=0; i<meshInfo.vertexCount; i++) {
		dv.writeFloat(data.v[i].pos[0]); dv.writeFloat(data.v[i].pos[1]); dv.writeFloat(data.v[i].pos[2]);
	};
	for (let i=0; i<meshInfo.vertexCount; i++) {
		dv.writeFloat(data.v[i].nm[0]); dv.writeFloat(data.v[i].nm[1]); dv.writeFloat(data.v[i].nm[2]);
	};
	for (let i=0; i<meshInfo.vertexCount; i++) {
		dv.writeFloat(data.v[i].uv[0]); dv.writeFloat(data.v[i].uv[1]);
	};
	for (let i=0; i<meshInfo.vertexCount; i++) {
		dv.writeFloat(data.v[i].w1[0]); dv.writeFloat(data.v[i].w1[1]); dv.writeFloat(data.v[i].w1[2]);
	};
	for (let i=0; i<meshInfo.vertexCount; i++) {
		dv.writeFloat(data.v[i].w2[0]); dv.writeFloat(data.v[i].w2[1]); dv.writeFloat(data.v[i].w2[2]);
	};
	for (let i=0; i<meshInfo.indexCount; i++) {
		dv.writeInt16(data.f[i]);
	}
}
RmbClass.prototype.getMeshInfo = function (meshId) {
	var mesh = this.meshes[meshId];
	return [mesh.index, mesh.name, mesh.parent, mesh.extraFlag, mesh.unknown2, mesh.unknown3, mesh.vertexCount, mesh.indexCount/3].join(' ');
}
RmbClass.prototype.dumpMeshData = function ( data ) {
	var s = [];
	for (let i=0; i<data.length; i++) {
		s.push([
			'p',			data[i].pos[0].toFixed(5), data[i].pos[1].toFixed(5), data[i].pos[2].toFixed(5),
			'n',			data[i].nm[0].toFixed(5), data[i].nm[1].toFixed(5), data[i].nm[2].toFixed(5),
			'u',			data[i].uv[0].toFixed(5), data[i].uv[1].toFixed(5),
			'?',			data[i].w1[0].toFixed(5), data[i].w1[1].toFixed(5), data[i].w1[2].toFixed(5),
			'?',			data[i].w2[0].toFixed(5), data[i].w2[1].toFixed(5), data[i].w2[2].toFixed(5),
			].join(' '));
	}
	return s.join("\r\n");
}
RmbClass.prototype.dumpMeshJson = function ( data ) {
	var v=[], uv=[], n=[], f=[], nb=[], nt=[];
	data.v.forEach(function(d) {
		v.push(d.pos[0], d.pos[1], d.pos[2]);
		n.push(d.nm[0], d.nm[1], d.nm[2]);
		uv.push(d.uv[0], d.uv[1]);
		nb.push(d.w1[0], d.w1[1], d.w1[2]);
		nt.push(d.w2[0], d.w2[1], d.w2[2]);
	});
	data.f.forEach(function(d) {
		f.push(d);
	});

	return {v, uv, n, f, nb, nt};
}
RmbClass.prototype.importObj = function ( meshId, obj ) {
	const sour = obj.mesh;
	const mesh = this.meshdata[meshId];
	mesh.f = [];
	var vert = [];

	sour.f.every(function(v, fid) {
		var fa = [], tmp;
		for (var i=0; i<3; i++) {
			mesh.f.push(v[i]);
			fa.push(v[i]);
			var ni = v[i+6] ? v[i+6] : v[i];

			if (!vert[v[i]]) {
				vert[v[i]] = {
					pos: new Float3(sour.v[v[i]][0], sour.v[v[i]][1], sour.v[v[i]][2]),
					uv: sour.u[ (v[i+3] ? v[i+3] : v[i]) ],
					nm: new Float3(sour.n[ni][0], sour.n[ni][1], sour.n[ni][2]),
					nt: new Float3(0, 0, 0),   //??   tangent
					nb: new Float3(0, 0, 0),   //??   binormal
				};
				vert[v[i]].uv[1] = 1 - vert[v[i]].uv[1];
			}
		}

		tmp = triangleMkNormal([vert[fa[0]].pos, vert[fa[0]].uv[0], vert[fa[0]].uv[1], vert[fa[0]].nm ], 
								[vert[fa[1]].pos, vert[fa[1]].uv[0], vert[fa[1]].uv[1], vert[fa[1]].nm ], 
								[vert[fa[2]].pos, vert[fa[2]].uv[0], vert[fa[2]].uv[1], vert[fa[2]].nm ], 
								true);
		vert[fa[0]].nb.Plus(tmp[1]);		vert[fa[0]].nt.Plus(tmp[2]);

		tmp = triangleMkNormal([vert[fa[1]].pos, vert[fa[1]].uv[0], vert[fa[1]].uv[1], vert[fa[1]].nm ], 
								[vert[fa[2]].pos, vert[fa[2]].uv[0], vert[fa[2]].uv[1], vert[fa[2]].nm ], 
								[vert[fa[0]].pos, vert[fa[0]].uv[0], vert[fa[0]].uv[1], vert[fa[0]].nm ], 
								true);
		vert[fa[1]].nb.Plus(tmp[1]);		vert[fa[1]].nt.Plus(tmp[2]);

		tmp = triangleMkNormal([vert[fa[2]].pos, vert[fa[2]].uv[0], vert[fa[2]].uv[1], vert[fa[2]].nm ], 
								[vert[fa[0]].pos, vert[fa[0]].uv[0], vert[fa[0]].uv[1], vert[fa[0]].nm ], 
								[vert[fa[1]].pos, vert[fa[1]].uv[0], vert[fa[1]].uv[1], vert[fa[1]].nm ], 
								true);
		vert[fa[2]].nb.Plus(tmp[1]);		vert[fa[2]].nt.Plus(tmp[2]);

		//if (fid>5) return false;
		return true;
	});


	mesh.v = [];
	vert.forEach(function(v) {
		v.nt.normalize();
		v.nb.normalize();
		mesh.v.push({
			pos: [v.pos.x, v.pos.y, v.pos.z],
			nm: [v.nm.x, v.nm.y, v.nm.z],
			uv: v.uv,
			w1: [v.nt.x, v.nt.y, v.nt.z],
			w2: [v.nb.x, v.nb.y, v.nb.z],
		});
	})

	this.meshes[meshId].vertexCount = mesh.v.length;
	this.meshes[meshId].indexCount = mesh.f.length;
}


function ObjClass(name) {
	this.dv = [];
	this.name = name;
	this.mesh = {v:[], n:[], u:[], f:[]};
	this.prec = 6;
};
ObjClass.prototype.parseFile = function ( filename ) {
	try {
		if (! fs.statSync(filename) ) throw new Error('No file: ' + filename);
	} catch (ex) {
		throw new Error('No file: ' + filename)
	}

	var array = fs.readFileSync(filename).toString().split("\n");
	for(let i in array) {
		this.parseLine(array[i]);
	}
}
ObjClass.prototype.parseLine = function (v) {
	v = v.trim();
	if (v.substring(0,2) === 'v ') {
		let w = v.split(' ');
		this.mesh.v.push([parseFloat(w[1]), parseFloat(w[2]), parseFloat(w[3]) ]);
	}
	if (v.substring(0,3) === 'vn ') {
		let w = v.split(' ');
		this.mesh.n.push([parseFloat(w[1]), parseFloat(w[2]), parseFloat(w[3]) ]);
	}
	if (v.substring(0,3) === 'vt ') {
		let w = v.split(' ');
		this.mesh.u.push([parseFloat(w[1]), parseFloat(w[2]) ]);
	}
	if (v.substring(0,2) === 'f ') {
		let w = v.split(' ');
		let w1 = w[1].split('/');
		let w2 = w[2].split('/');
		let w3 = w[3].split('/');
		if (w.length>4) throw new Error('Only triangular polygon');
		let f = [parseInt(w1[0])-1, parseInt(w2[0])-1, parseInt(w3[0])-1 ];
		if (w1[1]) {
			f.push(parseInt(w1[1])-1, parseInt(w2[1])-1, parseInt(w3[1])-1);
		}
		if (w1[2]) {
			f.push(parseInt(w1[2])-1, parseInt(w2[2])-1, parseInt(w3[2])-1);
		}
		this.mesh.f.push(f);
	}
}
ObjClass.prototype.getInfo = function (idx) {
	var mesh = this.mesh;
	return ['vertex=', mesh.v.length, 'normals=', mesh.n.length, 'uv=', mesh.u.length, 'faces=', mesh.f.length].join(' ');
}
ObjClass.prototype.wln = function (v) {
	this.dv.push(v);
}
ObjClass.prototype.serialize = function () {
	this.wln('# rmb exporter');
	this.wln('o ' + this.name);
	this.mesh.v.forEach( v => this.wln('v ' + v[0].toFixed(this.prec) + ' ' + v[1].toFixed(this.prec) + ' ' + v[2].toFixed(this.prec)) );
	this.mesh.n.forEach( v => this.wln('vn ' + v[0].toFixed(this.prec) + ' ' + v[1].toFixed(this.prec) + ' ' + v[2].toFixed(this.prec)) );
	this.mesh.u.forEach( v => this.wln('vt ' + v[0].toFixed(this.prec) + ' ' + v[1].toFixed(this.prec) ) );
	this.wln('s 1');
	this.mesh.f.forEach( v => this.wln('f ' + v[0]+'/'+v[0]+'/'+v[0] +' ' + v[1]+'/'+v[1]+'/'+v[1] + ' ' + v[2]+'/'+v[2]+'/'+v[2]) );

	return this.dv.join("\r\n");
}



function triangleMkNormal(v1, v2, v3, g=false) {    //v[f3, u, v, n]
	var a, b, r, normal;
	var v12 = v2[0].clone(); v12.Minus(v1[0]);
	var v13 = v3[0].clone(); v13.Minus(v1[0]);

	v12.normalize();
	v13.normalize();

	if (g) {
		normal = v1[3].clone();
	} else {
		normal = v12.crossProduct(v13);
	}

	const x1 = v2[0].x - v1[0].x;
	const x2 = v3[0].x - v1[0].x;
	const y1 = v2[0].y - v1[0].y;
	const y2 = v3[0].y - v1[0].y;
	const z1 = v2[0].z - v1[0].z;
	const z2 = v3[0].z - v1[0].z;
	
	const s1 = v2[1] - v1[1];
	const s2 = v3[1] - v1[1];
	const t1 = v2[2] - v1[2];
	const t2 = v3[2] - v1[2];

	const d = (s1 * t2 - s2 * t1);
	if (d == 0) {
		r = 1;
	} else {
		r = 1.0 / d;
	}

	const sdir = new Float3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r );
	const tdir = new Float3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r );

	//tangent = normalize (sdir - normal * (dot normal sdir))
	var tangent = sdir.clone(); a = normal.dotProduct(sdir);
	b = normal.clone();
	tangent.Minus(b.Umn(a)).normalize();

	//handeness = dot (cross normal sdir) tdir
	a = normal.clone(); a=a.crossProduct(sdir);
	var handeness = a.dotProduct(tdir);

	if (handeness < 0) {   handeness = -1; } else {   handeness = 1; }
	
	//bitangent = normalize ((cross normal sdir) * handeness)
	var bitangent = normal.clone(); bitangent = bitangent.crossProduct(sdir); 
	bitangent.Umn(handeness).normalize();

	return [normal, bitangent, tangent];
}


function rmbExport(rmb) {
	for (let meshId = 0; meshId < rmb.meshes.length; meshId++) {
		var name = rmb.meshes[meshId].name;
		var obj = new ObjClass(name);
		rmbExportMesh(rmb, obj, 0);
		var filename = name + '.obj';
		slog('Write: ' + filename);
		fs.writeFileSync(filename, obj.serialize());
	}
}
function rmbExportMesh(rmb, obj, idx) {
	var mesh = rmb.meshdata[idx];

	mesh.v.forEach( function (v) {
		obj.mesh.v.push([v.pos[0], v.pos[1], v.pos[2] ]);
		obj.mesh.n.push([v.nm[0], v.nm[1], v.nm[2] ]);
		obj.mesh.u.push([v.uv[0], 1-v.uv[1] ]);
	});
	for (let i=0; i<mesh.f.length; i+=3) {
		obj.mesh.f.push([1+mesh.f[i], 1+mesh.f[i+1], 1+mesh.f[i+2] ]);
	}
}
function slog(t) {
	console.log(t);
}
function rmbImport(rmb, filename) {
	var obj = new ObjClass();
	slog('Reading file: ' + filename);
	obj.parseFile(filename);
	slog(obj.getInfo());
	slog('Importing...');
	var meshId = 0;
	rmb.importObj(meshId, obj);
	slog(rmb.getMeshInfo(meshId));

	var newname = rmb.filename.substring(0, rmb.filename.length-4) + '.new' + rmb.filename.substring(rmb.filename.length-4);
	var buf = rmb.serialize();
	slog('Write: '+newname);
	fs.writeFileSync(newname, buf, 'binary');
}

function fileGetExt(t) {
	return t.substring(t.length - 4).toLowerCase();
}





slog('rmb exporter/importer    ver=' + Version);

function maincom(arg) {
	var rmb = new RmbClass();

	if (process.argv[2] && fileGetExt(process.argv[2]) == '.rmb') {
		try {
			rmb.parseFile(process.argv[2]);
		} catch (ex) {
			slog(ex.toString());
		}

		for (let i=0; i<rmb.meshes.length; i++) {
			slog(rmb.getMeshInfo(i));
		}

		if (!process.argv[3]) {
			slog('Exporting...');
			try {
				rmbExport(rmb);
			} catch (ex) {
				slog('Error export: ' + ex.toString());
			}
		}
		if (process.argv[3] && fileGetExt(process.argv[3]) == '.obj') {
			try {
				rmbImport(rmb, process.argv[3]);
			} catch (ex) {
				slog('Error import: ' + ex.toString());
			}
		}

	}

}


try {
	maincom(process.argv);
} catch (ex) {
	slog('Error');
}




// fs.writeFileSync('raw.raw', rmb.dumpMeshData(rmb.meshdata[0].v));
// fs.writeFileSync('mesh.json', JSON.stringify(rmb.dumpMeshJson(rmb.meshdata[0])));




