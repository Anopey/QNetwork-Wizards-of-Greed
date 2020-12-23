package packet

import "reflect"

type PacketData struct {
	id         int
	primitives []reflect.Type
}

