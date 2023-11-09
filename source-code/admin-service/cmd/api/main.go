package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
)

const webPort = "80"

type Config struct {
	Server struct {
		URL string `json:"url"`
	}
}

func main() {
	app, err := loadConfig("./config.json")
	if err != nil {
		log.Panic(err)
	}

	log.Println("starting service on port", webPort)
	srv := &http.Server{
		Addr:    fmt.Sprintf(":%s", webPort),
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
