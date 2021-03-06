package packet

import (
	"math"
)

type Packet struct {
	buf            []byte
	ID             uint16
	readWritePos   uint16
	expectedLength uint16
}

func NewPacket(ID uint16) *Packet {
	p := Packet{
		buf:            make([]byte, 0),
		ID:             ID,
		readWritePos:   0,
		expectedLength: GetPacketManager().GetPacketDataType(ID).MinimumByteLength,
	}
	p.WriteUInt16(ID)
	p.WriteUInt16(p.expectedLength)
	return &p
}

func NewPacketWithStrings(ID uint16, strings []string) *Packet {
	p := Packet{
		buf:          make([]byte, 0),
		ID:           ID,
		readWritePos: 0,
	}
	p.WriteUInt16(ID)
	length := (GetPacketManager().GetPacketDataType(ID).MinimumByteLength)

	for _, str := range strings {
		length += uint16(len(str))
	}

	p.WriteUInt16(length)
	p.writeStrings(strings)

	p.expectedLength = length
	return &p
}

func NewPacketByCopy(buf []byte) *Packet {
	p := Packet{
		buf:          make([]byte, len(buf)),
		readWritePos: 0,
	}
	copy(p.buf, buf)
	p.ID = p.ReadUInt16()
	p.expectedLength = p.ReadUInt16()
	return &p
}

func NewPacketBySameReference(buf []byte) *Packet {
	p := Packet{
		buf:          buf,
		readWritePos: 0,
	}
	p.ID = p.ReadUInt16()
	p.expectedLength = p.ReadUInt16()
	return &p
}

//region Utils

func (p *Packet) ExpectedLength() uint16 {
	return p.expectedLength
}

func (p *Packet) BufferLength() uint16 {
	return uint16(len(p.buf))
}

func (p *Packet) GetAllBytes() []byte {
	return p.buf
}

//endregion

//region Write Data

//assumes Little Endian!

func (p *Packet) WriteByte(val byte) {
	p.buf = append(p.buf, val)
}

func (p *Packet) WriteBytes(vals []byte) {
	p.buf = append(p.buf, vals...)
}

func (p *Packet) WriteUInt16(val uint16) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
}

func (p *Packet) WriteInt16(val int16) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
}

func (p *Packet) WriteInt32(val int32) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
	p.buf = append(p.buf, byte(val>>16))
	p.buf = append(p.buf, byte(val>>24))
}

func (p *Packet) WriteInt64(val int64) {
	p.buf = append(p.buf, byte(val))
	p.buf = append(p.buf, byte(val>>8))
	p.buf = append(p.buf, byte(val>>16))
	p.buf = append(p.buf, byte(val>>24))
	p.buf = append(p.buf, byte(val>>32))
	p.buf = append(p.buf, byte(val>>40))
	p.buf = append(p.buf, byte(val>>48))
	p.buf = append(p.buf, byte(val>>56))
}

func (p *Packet) WriteFloat32(val float32) {
	f := math.Float32bits(val)
	p.WriteInt32(int32(f))
}

func (p *Packet) writeFloat64(val float64) {
	d := math.Float64bits(val)
	p.WriteInt64(int64(d))
}

func (p *Packet) WriteBool(val bool) {
	if val {
		p.buf = append(p.buf, 1)
	} else {
		p.buf = append(p.buf, 0)
	}
}

func (p *Packet) writeStrings(val []string) {
	for _, str := range val {
		p.WriteUInt16(uint16(len(str)))
		p.buf = append(p.buf, str...)
	}
}

//endregion

//region Reading

//TODO: perhaps add length checks etc.

func (p *Packet) ReadByte() byte {
	p.readWritePos++
	return p.buf[p.readWritePos-1]
}

func (p *Packet) ReadBytes(len uint16) []byte {
	p.readWritePos += len
	return p.buf[p.readWritePos-len : p.readWritePos]
}

func (p *Packet) ReadUInt16() uint16 {
	return uint16(p.ReadByte()) | uint16(p.ReadByte())<<8
}

func (p *Packet) ReadInt16() int16 {
	return int16(uint16(p.ReadByte()) | uint16(p.ReadByte())<<8)
}

func (p *Packet) ReadInt32() int32 {
	return int32(uint32(p.ReadByte()) | uint32(p.ReadByte())<<8 | uint32(p.ReadByte())<<16 | uint32(p.ReadByte())<<24)
}

func (p *Packet) ReadInt64() int64 {
	return int64(uint64(p.ReadByte()) | uint64(p.ReadByte())<<8 | uint64(p.ReadByte())<<16 | uint64(p.ReadByte())<<24 |
		uint64(p.ReadByte())<<32 | uint64(p.ReadByte())<<40 | uint64(p.ReadByte())<<48 | uint64(p.ReadByte())<<56)
}

func (p *Packet) ReadFloat32() float32 {
	return math.Float32frombits(uint32(p.ReadByte()) | uint32(p.ReadByte())<<8 | uint32(p.ReadByte())<<16 | uint32(p.ReadByte())<<24)
}

func (p *Packet) ReadFloat64() float64 {
	return math.Float64frombits(uint64(p.ReadByte()) | uint64(p.ReadByte())<<8 | uint64(p.ReadByte())<<16 | uint64(p.ReadByte())<<24 |
		uint64(p.ReadByte())<<32 | uint64(p.ReadByte())<<40 | uint64(p.ReadByte())<<48 | uint64(p.ReadByte())<<56)
}

func (p *Packet) ReadBool() bool {
	p.readWritePos++
	if p.buf[p.readWritePos-1] == 0 {
		return false
	}
	return true
}

func (p *Packet) ReadString() string {
	len := p.ReadUInt16()
	return string(p.ReadBytes(len))
}

//endregion
