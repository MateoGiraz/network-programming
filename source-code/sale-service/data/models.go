package data

type Sale struct {
	Product  Product `json:"product"`
	Username string  `json:"username"`
}

type Product struct {
	Name        string `json:"name"`
	Description string `json:"description"`
	Price       int    `json:"price"`
}

type Filter struct {
	Name        string `json:"name,omitempty"`
	Username    string `json:"username,omitempty"`
	Description string `json:"description,omitempty"`
}
