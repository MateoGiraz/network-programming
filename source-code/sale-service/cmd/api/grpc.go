package main

import (
	"context"
	"fmt"
	"github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data"
	repo "github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data/repository"
	"github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/sale"
	"google.golang.org/grpc"
	"log"
	"net"
)

const gRpcPort = "50001"

type SaleServer struct {
	sale.UnimplementedSaleServiceServer
	repo repo.SaleRepository
}

func (s *SaleServer) CreateSale(ctx context.Context, req *sale.Sale) (*sale.SaleResponse, error) {
	product := req.GetProduct()
	username := req.GetUsername()

	newSale := data.Sale{
		Product: data.Product{
			Name:        product.Name,
			Description: product.Description,
			Price:       int(product.Price),
		},
		Username: username,
	}

	s.repo.Insert(newSale)

	res := &sale.SaleResponse{Result: fmt.Sprintf("Created %s's sale", username)}
	return res, nil
}

func (app *Config) gRPCListen() {
	lis, err := net.Listen("tcp", fmt.Sprintf(":%s", gRpcPort))
	if err != nil {
		log.Panic(err)
	}

	s := grpc.NewServer()

	sale.RegisterSaleServiceServer(s, &SaleServer{repo: app.repo})
	log.Printf("gRPC server started on port %s", gRpcPort)

	if err := s.Serve(lis); err != nil {
		log.Panic(err)
	}

}
