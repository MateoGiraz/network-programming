package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"

	repo "github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data/repository"
)

const webPort = "8080"

type Config struct {
	repo repo.SaleRepository
}

type AppConfig struct {
	Grpc struct {
		Port string `json:"port"`
	}
	Port string `json:"port"`
}

func main() {
	app := Config{
		repo: &repo.MemoryRepository{},
	}

	appConfig, err := loadConfig("./config.json")
	var grpcPort string

	if err != nil {
		grpcPort = ""
	} else {
		grpcPort = appConfig.Grpc.Port
	}

	go app.gRPCListen(grpcPort)

	port := webPort
	if appConfig.Port != "" {
		port = appConfig.Port
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

func loadConfig(filename string) (*AppConfig, error) {
	var app AppConfig
	configFile, err := os.ReadFile(filename)
	if err != nil {
		return &AppConfig{}, err
	}

	err = json.Unmarshal(configFile, &app)
	if err != nil {
		return &AppConfig{}, err
	}
	return &app, err
}
