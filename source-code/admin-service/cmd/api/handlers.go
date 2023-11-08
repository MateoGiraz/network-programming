package main

import (
	"context"
	"errors"
	"log"
	"net/http"
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
	var request RequestPayload
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

	log.Print(request)

	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	res, err := grpcFunc(ctx, &product.ProductIdentifier{
		Name: request.Name,
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

func (app *Config) getRating(w http.ResponseWriter, r *http.Request) {
	var request RequestPayload
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

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
		Name: request.Name,
		Credentials: &product.Credentials{
			Username: request.Credentials.Username,
			Password: request.Credentials.Password,
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
		"172.29.1.246:5001", grpc.WithTransportCredentials(insecure.NewCredentials()))

	return connection, err
}
