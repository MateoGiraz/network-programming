package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
)

const webPort = "8081"

type Config struct {
	GrpcServer struct {
		URL string `json:"url"`
	}
	Port string `json:"port"`
}

func main() {
	app, err := loadConfig("./config.json")
	if err != nil {
		log.Panic(err)
	}

	port := webPort
	if app.Port != "" {
		port = app.Port
	}

	log.Println("starting service on port", port)
	srv := &http.Server{
		Addr:    fmt.Sprintf(":%s", port),
		Handler: app.routes(),
	}

	err = srv.ListenAndServe()
	if err != nil {
		log.Panic(err)
	}
}

func loadConfig(filename string) (*Config, error) {
	var app Config
	configFile, err := os.ReadFile(filename)
	if err != nil {
		return &Config{}, err
	}

	err = json.Unmarshal(configFile, &app)
	if err != nil {
		return &Config{}, err
	}
	return &app, err
}
