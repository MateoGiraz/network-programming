FROM golang:1.19-alpine as builder

RUN mkdir /app

COPY . /app

WORKDIR /app

RUN GOOS=linux CGO_ENABLED=0 go build -o adminServiceApp ./cmd/api

FROM alpine:latest

RUN mkdir /app

COPY --from=builder /app/adminServiceApp /app

CMD ["/app/adminServiceApp"]