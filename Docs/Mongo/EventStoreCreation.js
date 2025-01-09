
const database = 'ERPEventStore';
const collection = 'PurchaseOrder';

// Create a new database.
use(database);

// Create a new collection.
db.createCollection(collection);
