package repository

import "github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data"

type SaleRepository interface {
	Insert(sale data.Sale)
	Get(filter data.Filter) []data.Sale
}
