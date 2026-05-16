'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/lib/api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { toast } from 'sonner';

const categories = ['Art', 'Photography', 'Music', 'Design', 'Font', 'Other'];

export default function CreateProductPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({
    title: '',
    description: '',
    price: '',
    fileUrl: '',
    category: 0,
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await api.post('/products', {
        ...form,
        price: parseFloat(form.price),
      });
      toast.success('Продуктът е създаден!');
      router.push('/products');
    } catch {
      toast.error('Грешка при създаване на продукта.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-2xl mx-auto">
        <Button variant="outline" className="mb-6" onClick={() => router.back()}>
          ← Назад
        </Button>

        <Card>
          <CardHeader>
            <CardTitle>Нов продукт</CardTitle>
          </CardHeader>
          <form onSubmit={handleSubmit}>
            <CardContent className="space-y-4">
              <div className="space-y-2">
                <Label>Заглавие</Label>
                <Input
                  value={form.title}
                  onChange={(e) => setForm({ ...form, title: e.target.value })}
                  placeholder="Abstract Art Pack"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label>Описание</Label>
                <Input
                  value={form.description}
                  onChange={(e) => setForm({ ...form, description: e.target.value })}
                  placeholder="Описание на продукта..."
                  required
                />
              </div>

              <div className="space-y-2">
                <Label>Цена ($)</Label>
                <Input
                  type="number"
                  step="0.01"
                  value={form.price}
                  onChange={(e) => setForm({ ...form, price: e.target.value })}
                  placeholder="19.99"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label>File URL</Label>
                <Input
                  value={form.fileUrl}
                  onChange={(e) => setForm({ ...form, fileUrl: e.target.value })}
                  placeholder="https://example.com/file.zip"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label>Категория</Label>
                <select
                  className="w-full border rounded-md p-2 text-sm"
                  value={form.category}
                  onChange={(e) => setForm({ ...form, category: parseInt(e.target.value) })}
                >
                  {categories.map((cat, index) => (
                    <option key={cat} value={index}>{cat}</option>
                  ))}
                </select>
              </div>

              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? 'Създаване...' : 'Създай продукт'}
              </Button>
            </CardContent>
          </form>
        </Card>
      </div>
    </div>
  );
}