package main

import (
	"context"
	"errors"
	"fmt"
	"log"
	"net/http"
	"strings"
	"time"

	"github.com/ORT-PDR/M6C_241195_256345_231355/admin-service/product"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type RequestPayload struct {
	Name        string      `json:"name"`
	Description string      `json:"description"`
	Stock       int         `json:"stock"`
	Price       int         `json:"price"`
	Credentials Credentials `json:"credentials"`
}

type Credentials struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type ProductPayloadFunc func(ctx context.Context, in *product.Product, opts ...grpc.CallOption) (*product.ProductResponse, error)
type ProductIdentifierPayloadFunc func(ctx context.Context, in *product.ProductIdentifier, opts ...grpc.CallOption) (*product.ProductResponse, error)

func (app *Config) createProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	log.Print("Connection established")
	c := product.NewProductServiceClient(connection)
	app.HandleProductRequest(w, r, c.CreateProduct)
}

func (app *Config) updateProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewProductServiceClient(connection)
	app.HandleProductRequest(w, r, c.UpdateProduct)
}

func (app *Config) deleteProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewProductServiceClient(connection)
	app.HandleProductIdentifierRequest(w, r, c.DeleteProduct)
}

func (app *Config) buyProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewProductServiceClient(connection)
	app.HandleProductIdentifierRequest(w, r, c.BuyProduct)
}

func (app *Config) HandleProductRequest(w http.ResponseWriter, r *http.Request, grpcFunc ProductPayloadFunc) {
	var request RequestPayload
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

	log.Print(request)

	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	res, err := grpcFunc(ctx, &product.Product{
		Name:        request.Name,
		Description: request.Description,
		Stock:       int32(request.Stock),
		Price:       int32(request.Price),
		Credentials: &product.Credentials{
			Username: request.Credentials.Username,
			Password: request.Credentials.Password,
		},
	})

	if err != nil {
		app.errorJSON(w, errors.New(res.Result))
		return
	}

	app.writeJSON(w, http.StatusAccepted, res)
}

func (app *Config) HandleProductIdentifierRequest(w http.ResponseWriter, r *http.Request, grpcFunc ProductIdentifierPayloadFunc) {
	var request Credentials
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

	parts := strings.Split(r.URL.Path, "/")
	if len(parts) >= 3 {
		id := parts[2]
		fmt.Printf("ID: %s\n", id)
	} else {
		http.Error(w, "Invalid URL", http.StatusBadRequest)
	}

	name := parts[2]
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	res, err := grpcFunc(ctx, &product.ProductIdentifier{
		Name: name,
		Credentials: &product.Credentials{
			Username: request.Username,
			Password: request.Password,
		},
	})

	if err != nil {
		app.errorJSON(w, errors.New(res.Result))
		return
	}

	app.writeJSON(w, http.StatusAccepted, res)
}

func (app *Config) getRating(w http.ResponseWriter, r *http.Request) {
	var request Credentials
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

	parts := strings.Split(r.URL.Path, "/")
	if len(parts) >= 3 {
		id := parts[2]
		fmt.Printf("ID: %s\n", id)
	} else {
		http.Error(w, "Invalid URL", http.StatusBadRequest)
	}

	name := parts[2]

	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewProductServiceClient(connection)
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	res, err := c.GetRating(ctx, &product.ProductIdentifier{
		Name: name,
		Credentials: &product.Credentials{
			Username: request.Username,
			Password: request.Password,
		},
	})

	if err != nil {
		app.errorJSON(w, errors.New(string(res.Code)))
		return
	}

	app.writeJSON(w, http.StatusAccepted, res)
}

func GetConnection() (*grpc.ClientConn, error) {
	connection, err := grpc.Dial(
		"192.168.1.80:5001", grpc.WithTransportCredentials(insecure.NewCredentials()))

	return connection, err
}
