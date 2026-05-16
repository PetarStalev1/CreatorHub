export interface User {
  id: string;
  email: string;
  displayName: string;
  avatarUrl?: string;
  role: 'Buyer' | 'Creator' | 'Admin';
  createdAt: string;
}

export interface Product {
  id: string;
  title: string;
  description: string;
  price: number;
  fileUrl: string;
  category: string;
  downloadCount: number;
  sellerName: string;
  createdAt: string;
}

export interface Order {
  id: string;
  totalAmount: number;
  status: string;
  items: OrderItem[];
  createdAt: string;
}

export interface OrderItem {
  productId: string;
  productTitle: string;
  priceAtPurchase: number;
}

export interface Review {
  id: string;
  productId: string;
  reviewerName: string;
  rating: number;
  comment?: string;
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  displayName: string;
  role: string;
}