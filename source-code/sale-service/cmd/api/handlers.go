package main

import (
	"github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data"
	"log"
	"net/http"
)

func (app *Config) getSales(w http.ResponseWriter, r *http.Request) {
	username := r.URL.Query().Get("username")
	name := r.URL.Query().Get("name")
	description := r.URL.Query().Get("description")

	filter := data.Filter{
		name,
		username,
		description,
	}

	log.Println(filter.Name)

	sales := app.repo.Get(filter)
	app.writeJSON(w, http.StatusAccepted, sales)
}
