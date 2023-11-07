package main

import (
	"context"
	"github.com/ORT-PDR/M6C_241195_256345_231355/admin-service/product"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
	"net/http"
	"time"
)

var conn *grpc.ClientConn

type RequestPayload struct {
	Name        string
	Description string
	Stock       int
	Price       int
	Credentials Credentials
}

type Credentials struct {
	Username string
	Password string
}

type GrpcFunc func(ctx context.Context, in *product.Product, opts ...grpc.CallOption) (*product.ProductResponse, error)

func (app *Config) createProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewSaleServiceClient(connection)
	app.HandleRequest(w, r, c.CreateProduct)
}

func (app *Config) updateProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewSaleServiceClient(connection)
	app.HandleRequest(w, r, c.UpdateProduct)
}

func (app *Config) deleteProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewSaleServiceClient(connection)
	app.HandleRequest(w, r, c.DeleteProduct)
}

func (app *Config) buyProduct(w http.ResponseWriter, r *http.Request) {
	connection, err := GetConnection()
	if err != nil {
		app.errorJSON(w, err)
		return
	}
	defer connection.Close()

	c := product.NewSaleServiceClient(connection)
	app.HandleRequest(w, r, c.BuyProduct)
}

func (app *Config) HandleRequest(w http.ResponseWriter, r *http.Request, grpcFunc GrpcFunc) {
	var request RequestPayload
	err := app.readJSON(w, r, &request)
	if err != nil {
		app.errorJSON(w, err)
		return
	}

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
		app.errorJSON(w, err)
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

	c := product.NewSaleServiceClient(connection)
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	res, err := c.GetRating(ctx, &product.Product{
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
		app.errorJSON(w, err)
		return
	}

	app.writeJSON(w, http.StatusAccepted, res)
}

func GetConnection() (*grpc.ClientConn, error) {
	if conn == nil {
		connection, err := grpc.Dial(
			"localhost:50001",
			grpc.WithTransportCredentials(insecure.NewCredentials()),
			grpc.WithBlock())

		conn = connection
		return conn, err
	}

	return conn, nil
}
