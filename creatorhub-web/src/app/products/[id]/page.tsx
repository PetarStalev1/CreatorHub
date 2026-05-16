'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import api from '@/lib/api';
import { Product, Review } from '@/types';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { toast } from 'sonner';

export default function ProductPage() {
  const { id } = useParams();
  const router = useRouter();
  const [product, setProduct] = useState<Product | null>(null);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [productRes, reviewsRes] = await Promise.all([
          api.get<Product>(`/products/${id}`),
          api.get<Review[]>(`/reviews/product/${id}`)
        ]);
        setProduct(productRes.data);
        setReviews(reviewsRes.data);
      } catch {
        toast.error('Продуктът не е намерен.');
        router.push('/products');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, router]);

  const handleOrder = async () => {
    try {
      await api.post('/orders', { productIds: [id] });
      toast.success('Продуктът е закупен успешно!');
    } catch {
      toast.error('Грешка при покупката. Влез в акаунта си.');
    }
  };

  const handleReview = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const { data } = await api.post<Review>('/reviews', {
        productId: id,
        rating,
        comment
      });
      setReviews([data, ...reviews]);
      setComment('');
      toast.success('Ревюто е добавено!');
    } catch {
      toast.error('Грешка при добавяне на ревю.');
    }
  };

  if (loading) return <div className="min-h-screen flex items-center justify-center"><p>Зареждане...</p></div>;
  if (!product) return null;

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-4xl mx-auto">
        <Button variant="outline" className="mb-6" onClick={() => router.back()}>
          ← Назад
        </Button>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div className="md:col-span-2 space-y-6">
            <Card>
              <CardHeader>
                <CardTitle className="text-2xl">{product.title}</CardTitle>
                <p className="text-gray-500">от {product.sellerName}</p>
              </CardHeader>
              <CardContent className="space-y-4">
                <p className="text-gray-700">{product.description}</p>
                <div className="flex gap-4 text-sm text-gray-500">
                  <span>Категория: {product.category}</span>
                  <span>{product.downloadCount} изтегляния</span>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Ревюта ({reviews.length})</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {reviews.length === 0 && (
                  <p className="text-gray-500">Няма ревюта все още.</p>
                )}
                {reviews.map((review) => (
                  <div key={review.id} className="border-b pb-3">
                    <div className="flex justify-between">
                      <span className="font-medium">{review.reviewerName}</span>
                      <span className="text-yellow-500">{'★'.repeat(review.rating)}{'☆'.repeat(5 - review.rating)}</span>
                    </div>
                    {review.comment && <p className="text-gray-600 text-sm mt-1">{review.comment}</p>}
                  </div>
                ))}

                <form onSubmit={handleReview} className="space-y-3 pt-4 border-t">
                  <h3 className="font-medium">Добави ревю</h3>
                  <div className="space-y-2">
                    <Label>Оценка (1-5)</Label>
                    <Input
                      type="number"
                      min={1}
                      max={5}
                      value={rating}
                      onChange={(e) => setRating(parseInt(e.target.value))}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Коментар</Label>
                    <Input
                      value={comment}
                      onChange={(e) => setComment(e.target.value)}
                      placeholder="Страхотен продукт!"
                    />
                  </div>
                  <Button type="submit" variant="outline" className="w-full">
                    Добави ревю
                  </Button>
                </form>
              </CardContent>
            </Card>
          </div>

          <div>
            <Card className="sticky top-8">
              <CardContent className="pt-6 space-y-4">
                <p className="text-3xl font-bold">${product.price}</p>
                <Button className="w-full" onClick={handleOrder}>
                  Купи сега
                </Button>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </div>
  );
}