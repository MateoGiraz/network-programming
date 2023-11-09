package main

import (
	"encoding/json"
	"fmt"
	repo "github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data/repository"
	"log"
	"net/http"
	"os"
)

const webPort = "80"

type Config struct {
	repo repo.SaleRepository
}

type AppConfig struct {
	Grpc struct {
		Port string `json:"port"`
	}
}

func main() {
	app := Config{
		repo: &repo.MemoryRepository{},
	}

	appConfig, err := loadConfig("./config.json")
	var port string

	if err != nil {
		port = ""
	} else {
		port = appConfig.Grpc.Port
	}

	go app.gRPCListen(port)

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
