package repository

import (
	"github.com/ORT-PDR/M6C_241195_256345_231355/sale-service/data"
	"strings"
)

type MemoryRepository struct {
	sales []data.Sale
}

func (repo *MemoryRepository) Insert(sale data.Sale) {
	repo.sales = append(repo.sales, sale)
}

func (repo *MemoryRepository) Get(filter data.Filter) []data.Sale {
	ret := repo.sales

	if filter.Name != "" {
		ret = filterSales(ret, func(s data.Sale) bool {
			return s.Product.Name == filter.Name
		})
	}

	if filter.Username != "" {
		ret = filterSales(ret, func(s data.Sale) bool {
			return s.Username == filter.Username
		})
	}

	if filter.Description != "" {
		ret = filterSales(ret, func(s data.Sale) bool {
			return strings.Contains(s.Product.Description, filter.Description)
		})
	}

	return ret
}

func filterSales[T any](ss []T, test func(T) bool) (ret []T) {
	for _, s := range ss {
		if test(s) {
			ret = append(ret, s)
		}
	}
	return
}
