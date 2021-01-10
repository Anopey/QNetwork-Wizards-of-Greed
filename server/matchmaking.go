package main

import (
	"sync"
)

const (
	maximumPlayersInQueue int = 8
)

type playerQueueInfo struct {
	p       *player
	isReady bool
}

var playersInQueue []*playerQueueInfo = make([]*playerQueueInfo, 8)

var queueMutex sync.RWMutex = sync.RWMutex{}

func getMatchmakingGeneralPlayerErrors(p *player) string {
	if !p.ableToPlay {
		return "Player's isAbleToPlay field is set to false!"
	}

	if p.activeGame != nil {
		return "Player is already in a game!"
	}

	return ""
}

func leaveQueue(p *player) string {

	err := getMatchmakingGeneralPlayerErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if !queueContainsPlayer(p) {
		return "The player is not queued up!"
	}

	//leave queue logic

	queueMutex.Unlock()

	return ""
}

func queueUp(p *player) string {

	err := getMatchmakingGeneralPlayerErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if len(playersInQueue) == maximumPlayersInQueue {
		return "The queue is full!"
	}

	//queue up logic

	queueMutex.Unlock()

	return ""
}

func readyUp(p *player) string {

	err := getMatchmakingGeneralPlayerErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	queueMutex.Unlock()

	return ""
}

func unready(p *player) string {

	err := getMatchmakingGeneralPlayerErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	queueMutex.Unlock()

	return ""
}

func queueContainsPlayer(p *player) bool {
	for _, c := range playersInQueue {
		if c.p == p {
			return true
		}
	}
	return false
}
