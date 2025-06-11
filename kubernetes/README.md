# Kubernetes Manifests

This folder contains Kubernetes manifests for all services and dependencies of the project.

---

## Usage

Change directory to this folder:

```bash
cd kubernetes
```

---

## Manifest Deployment Order

Apply manifests in the following order to ensure dependencies start correctly:

```bash
kubectl apply -f ./azurite/
kubectl apply -f ./postgres/
kubectl apply -f ./rabbitmq/
kubectl apply -f ./keycloak/
kubectl apply -f ./auth-service/
kubectl apply -f ./chat-service/
kubectl apply -f ./file-service/
kubectl apply -f ./post-service/
kubectl apply -f ./workout-service/
kubectl apply -f ./frontend/
kubectl apply -f ./gateway/
```

---

## Delete All Resources

Delete resources in reverse order to safely remove dependent services:

```bash
kubectl delete -f ./gateway/
kubectl delete -f ./frontend/
kubectl delete -f ./workout-service/
kubectl delete -f ./post-service/
kubectl delete -f ./file-service/
kubectl delete -f ./chat-service/
kubectl delete -f ./auth-service/
kubectl delete -f ./keycloak/
kubectl delete -f ./rabbitmq/
kubectl delete -f ./postgres/
kubectl delete -f ./azurite/
```
