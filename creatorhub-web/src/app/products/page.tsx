'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/lib/api';
import { Product } from '@/types';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';

export default function ProductsPage() {
  const router = useRouter();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const { data } = await api.get<Product[]>('/products');
        setProducts(data);
      } catch {
        console.error('Грешка при зареждане на продуктите.');
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <p>Зареждане...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-6xl mx-auto">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-3xl font-bold">Продукти</h1>
          <div className="flex gap-3">
            <Button variant="outline" onClick={() => router.push('/dashboard')}>
              Dashboard
            </Button>
            <Button onClick={() => router.push('/products/create')}>
              + Нов продукт
            </Button>
          </div>
        </div>

        {products.length === 0 ? (
          <div className="text-center py-20">
            <p className="text-gray-500 text-lg">Няма продукти все още.</p>
            <Button className="mt-4" onClick={() => router.push('/products/create')}>
              Създай първия продукт
            </Button>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {products.map((product) => (
              <Card key={product.id} className="hover:shadow-lg transition-shadow">
                <CardHeader>
                  <CardTitle className="text-lg">{product.title}</CardTitle>
                  <p className="text-sm text-gray-500">от {product.sellerName}</p>
                </CardHeader>
                <CardContent>
                  <p className="text-gray-600 text-sm line-clamp-2">{product.description}</p>
                  <div className="mt-3 flex justify-between items-center">
                    <span className="text-2xl font-bold">${product.price}</span>
                    <span className="text-sm text-gray-400">{product.category}</span>
                  </div>
                  <p className="text-xs text-gray-400 mt-1">
                    {product.downloadCount} изтегляния
                  </p>
                </CardContent>
                <CardFooter>
                  <Button
                    className="w-full"
                    onClick={() => router.push(`/products/${product.id}`)}
                  >
                    Виж продукта
                  </Button>
                </CardFooter>
              </Card>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}