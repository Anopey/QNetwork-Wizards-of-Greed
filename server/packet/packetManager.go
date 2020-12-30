package packet

import (
	"reflect"
	"strconv"
	"sync"
)

var once sync.Once

var singleton PacketManager

type PacketHandler func(pd *PacketDataType, p *Packet)

type Kind = reflect.Kind

type PacketDataType struct {
	Description       string
	ID                uint16
	Primitives        []Kind
	Handler           *PacketHandler
	MinimumByteLength uint16
}

type PacketManager struct {
	packets map[uint16]*PacketDataType
}

func GetPacketManager() *PacketManager {

	once.Do(func() {
		singleton = PacketManager{
			packets: make(map[uint16]*PacketDataType),
		}
	})

	return &singleton
}

func (pm *PacketManager) RegisterPacketDataType(p *PacketDataType) {
	_, exists := pm.packets[p.ID]
	if exists {
		panic("packet type of id " + strconv.Itoa(int(p.ID)) + " already exists!")
	}
	pm.packets[p.ID] = p
	p.calculateMinimumByteLength()
}

func (pm *PacketManager) GetPacketDataType(id uint16) *PacketDataType {
	return pm.packets[id]
}

func (pd *PacketDataType) calculateMinimumByteLength() {
	pd.MinimumByteLength = 4 //since we start with ID and length

	for _, k := range pd.Primitives {
		switch k {
		case reflect.Bool:
			pd.MinimumByteLength += 1
			break
		case reflect.Uint8: //byte or char
			pd.MinimumByteLength += 1
			break
		case reflect.Int8:
			pd.MinimumByteLength += 1
			break
		case reflect.Int16:
			pd.MinimumByteLength += 2
			break
		case reflect.Uint16:
			pd.MinimumByteLength += 2
			break
		case reflect.Int32:
			pd.MinimumByteLength += 4
			break
		case reflect.Uint32:
			pd.MinimumByteLength += 4
			break
		case reflect.Int64:
			pd.MinimumByteLength += 8
			break
		case reflect.Uint64:
			pd.MinimumByteLength += 8
			break
		case reflect.Float32:
			pd.MinimumByteLength += 4
			break
		case reflect.Float64:
			pd.MinimumByteLength += 8
			break
		case reflect.String: //the start of the string will be a uint16 to indicate its length.
			pd.MinimumByteLength += 2
			break
		}
	}

}
