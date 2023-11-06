package main

import (
	"fmt"
	repo "github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data/repository"
	"log"
	"net/http"
)

const webPort = "80"

type Config struct {
	repo repo.SaleRepository
}

func main() {
	app := Config{
		repo: &repo.MemoryRepository{},
	}

	go app.gRPCListen()

	log.Println("starting service on port", webPort)
	srv := &http.Server{
		Addr:    fmt.Sprintf(":%s", webPort),
		Handler: app.routes(),
	}

	err := srv.ListenAndServe()
	if err != nil {
		log.Panic(err)
	}
}
