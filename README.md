# Purchase Order Service

This document outlines the design and implementation details of the **Purchase Order Service** using **Domain-Driven Design (DDD)**, **GraphQL**, **Kafka**, and **MongoDB** for recording events.
  
---

![GitHub repo size](https://img.shields.io/github/repo-size/mohamedabotir/POContext?style=for-the-badge)
![GitHub Repo stars](https://img.shields.io/github/stars/mohamedabotir/POContext?style=social)
![example workflow](https://github.com/mohamedabotir/POContext/actions/workflows/dotnet.yml/badge.svg)
![Coverage](https://img.shields.io/badge/coverage-36%25-brightgreen)

## Architecture Overview

### Key Components

1. **Domain-Driven Design (DDD)**:
   - Focus on the core domain and its logic.
   - Use aggregates, entities, value objects, repositories, and services.

2. **GraphQL**:
   - Provides a flexible query language and runtime for APIs.
   - Enables clients to query purchase orders efficiently.

3. **Kafka**:
   - Acts as a message broker for event-driven communication.
   - Ensures reliable event publishing and processing.

4. **MongoDB**:
   - Used as the event store to persist raised events.
   - Stores purchase orders and their lifecycle events.

---

## Service Features

### 1. **Create Purchase Order**
   - Accepts a GraphQL mutation to create a new purchase order.
   - Algorithm with two modes for creating purchase number 
   - Validates input data against domain rules.
   - Publishes a `PurchaseOrderCreated` event to Kafka.

### 2. **Approve Purchase Order**
   - Accepts a GraphQL mutation to approve a purchase order.
   - Checks the state of the order using domain logic.
   - Publishes a `PurchaseOrderApproved` event to Kafka.

### 3. **Deactivated Purchase Order**(todo business logic already implemented only need raise event)
   - Accepts a GraphQL mutation to reject a purchase order.
   - Applies business rules for rejection.
   - Publishes a `PurchaseOrderRejected` event to Kafka.
### 4. **Close Purchase Order**
   - Handler Of Event Fired
   - invoke usecase of close purchase order
---

## Domain Model

### Entities
- **LineItem**:
  - Attributes: `Id`, `quantity`, `item`, `guid`, `CreatedOn`, `ModifiedOn`

### Value Objects
- **Money**: Represents monetary values with currency.
- **Item**:  Represents item being purchased.
- **Email**: Represents user email
- **User** : Represents customer or client
- **Address**: Represents address
- **Quantity** : Represents quantity and type of quantity 

### Aggregates
- **PoEntity**:
  - Attributes: `Id`,`rootGuid`, `customer`, `supplier`, `poNumber`, `PurchaseOrderStage`,`CreatedAt`, `UpdatedAt`
  - Methods: `DeActivate()`, `Activate()`,`ApprovePurchase()`,`ClosePurchaseOrder()`,`AddLineItems()`

### Repositories
- Interface: `IPurchaseOrderRepository`,`IEventRepository`

---
## GraphQL API Design

### Schema
```graphql
schema {
  query: PurchaseOrderQuery
}

type PurchaseOrderQuery {
  purchaseOrders: [PurchaseOrder]
  purchaseOrderByPurchaseOrderNumber(purchaseOrderNumber: String): PurchaseOrder
}

type PurchaseOrder {
  # The ID of the purchase order.
  id: Long!

  # The purchase order number.
  poNumberValue: String!

  # The total amount of the purchase order.
  moneyValue: Decimal!

  # The activation status of the purchase order as an integer.
  activationStatus: Int

  # The list of line items.
  lineItems: [LineItems]
}

scalar Long

scalar Decimal

type LineItems {
  # The ID of the line item.
  id: Long!

  # The name of the line item.
  quantityType: QuantityType!

  # The quantity of the line item.
  quantityValue: Int!

  # The name of the line item.
  name: String!

  # The price of the line item.
  price: Decimal!

  # The SKU of the line item.
  sKU: String!
}

enum QuantityType {
  KILO
  GRAM
  TAB
}
```

### Example Queries/Mutations

#### Get All Purchase Orders
```graphql
query {
  purchaseOrders {
    id
    poNumberValue
    moneyValue
    activationStatus
    lineItems {
      id
      name
      price
    }
  }
}
```

#### Get Purchase Order by Number
```graphql
query {
  purchaseOrderByPurchaseOrderNumber(purchaseOrderNumber: "PO-20250118-B458B") {
    id
    poNumberValue
    moneyValue
    activationStatus
    lineItems {
      id
      name
      quantityValue
      price
    }
  }
}
```
### GetTop7 by latest date , and cached 
```
query {
  top7PurchaseOrder {
    id
    poNumberValue
    moneyValue
    activationStatus
    lineItems {
      id
      name
      price
    }
  }
}
```
---

## Event Sourcing and Kafka

### Event Types
1. **PurchaseOrderCreated**:
   - Contains details about the created order.

2. **PurchaseOrderApproved**:
   - Fired when an order is approved.

3. **PurchaseOrderDeActivated(To-Do)**:
   - Raised when an order is DeActivated.

### Kafka Configuration
- Topic: `purchaseOrder`

### MongoDB Event Store
- Collection: `PurchaseOrder`
- Schema:
  ```json
  {
    "_id": "<EventId>",
    "purchaseOrderId": "<PurchaseOrderId>",
    "eventType": "<EventType>",
    "data": { <EventData> },
    "timestamp": "<Timestamp>"
  }
  ```

---

### Event Handling
- Event listeners consume events from Kafka.
- Events are persisted in MongoDB and used for building read models.

### Example Workflow
1. User creates a purchase order using Api.
2. Domain validates and creates the order.
3. Event is published to Kafka.
4. MongoDB stores the event.
--------------------
1. User Approved a purchase order using Api
2. Domain validates if order activated
3. Status changed to approved
4. PurchaseOrderApproved is published to kafka
5. Store Event into mongo 
---

## Future Enhancements
1. Implement retry mechanisms for failed Kafka message processing.
2. Add Redis to cache first 7 purchase orders 
3. add population script for purchase order  

---
![Coverage](https://img.shields.io/badge/coverage-36%25-brightgreen)
