FROM golang:1.18-alpine as builder

RUN mkdir /app

COPY . /app

WORKDIR /app

RUN GOOS=linux CGO_ENABLED=0 go build -o mailServiceApp .

FROM alpine:latest

RUN mkdir /app

COPY --from=builder /app/mailServiceApp /app

CMD ["/app/mailServiceApp"]