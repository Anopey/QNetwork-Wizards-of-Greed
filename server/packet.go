package main

import (
	"math"
)

type packet struct {
	buf     []byte
	ID      int32
	readPos uint16
}

func newPacket(ID int32) *packet {
	p := packet{
		buf: make([]byte, 0),
		ID:  ID,
	}
	p.writeInt32(ID)
	return &p
}

func newPacketByCopy(buf []byte) *packet {
	p := packet{
		buf: make([]byte, len(buf)),
	}
	copy(p.buf, buf)
	p.ID = p.readInt32()
	return &p
}

func newPacketBySameReference(buf []byte) *packet {
	p := packet{
		buf: buf,
	}
	p.ID = p.readInt32()
	return &p
}

//region Write Data

//assumes Little Endian!

func (p *packet) writeByte(val byte) {
	p.buf = append(p.buf, val)
}

func (p *packet) writeBytes(vals []byte) {
	for _, i := range vals {
		p.buf = append(p.buf, i)
	}
}

func (p *packet) writeInt16(val int16) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
}

func (p *packet) writeInt32(val int32) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
	p.buf = append(p.buf, byte(val>>16))
	p.buf = append(p.buf, byte(val>>24))
}

func (p *packet) writeInt64(val int64) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
	p.buf = append(p.buf, byte(val>>16))
	p.buf = append(p.buf, byte(val>>24))
	p.buf = append(p.buf, byte(val>>32))
	p.buf = append(p.buf, byte(val>>40))
	p.buf = append(p.buf, byte(val>>48))
	p.buf = append(p.buf, byte(val>>56))
}

func (p *packet) writeFloat32(val float32) {
	f := math.Float32bits(val)
	p.writeInt32(int32(f))
}

func (p *packet) writeFloat64(val float64) {
	d := math.Float64bits(val)
	p.writeInt64(int64(d))
}

func (p *packet) writeBool(val bool) {
	if val {
		p.buf = append(p.buf, 1)
	} else {
		p.buf = append(p.buf, 0)
	}
}

func (p *packet) writeString(val string) {
	for _, i := range []byte(val) {
		p.buf = append(p.buf, i)
	}
}

//endregion

//region Reading

//TODO: perhaps add length checks etc.

func (p *packet) readByte() byte {
	p.readPos++
	return p.buf[p.readPos-1]
}

func (p *packet) readBytes(len uint16) []byte {
	p.readPos += len
	return p.buf[p.readPos-len : p.readPos]
}

func (p *packet) readInt16() int16 {
	return int16(uint16(p.readByte()) | uint16(p.readByte())<<8)
}

func (p *packet) readInt32() int32 {
	return int32(uint32(p.readByte()) | uint32(p.readByte())<<8 | uint32(p.readByte())<<16 | uint32(p.readByte())<<24)
}

func (p *packet) readInt64() int64 {
	return int64(uint64(p.readByte()) | uint64(p.readByte())<<8 | uint64(p.readByte())<<16 | uint64(p.readByte())<<24 |
		uint64(p.readByte())<<32 | uint64(p.readByte())<<40 | uint64(p.readByte())<<48 | uint64(p.readByte())<<56)
}

func (p *packet) readFloat32() float32 {
	return math.Float32frombits(uint32(p.readByte()) | uint32(p.readByte())<<8 | uint32(p.readByte())<<16 | uint32(p.readByte())<<24)
}

func (p *packet) readFloat64() float64 {
	return math.Float64frombits(uint64(p.readByte()) | uint64(p.readByte())<<8 | uint64(p.readByte())<<16 | uint64(p.readByte())<<24 |
		uint64(p.readByte())<<32 | uint64(p.readByte())<<40 | uint64(p.readByte())<<48 | uint64(p.readByte())<<56)
}

func (p *packet) readBool() bool {
	p.readPos++
	if p.buf[p.readPos-1] == 0 {
		return false
	}
	return true
}

func (p *packet) readString(len uint16) string {
	return string(p.readBytes(len))
}

//endregion
