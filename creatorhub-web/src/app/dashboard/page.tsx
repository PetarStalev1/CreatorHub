'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/lib/api';
import { User } from '@/types';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

export default function DashboardPage() {
  const router = useRouter();
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    const stored = localStorage.getItem('user');
    if (!stored) {
      router.push('/login');
      return;
    }
    setUser(JSON.parse(stored));
  }, [router]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    router.push('/login');
  };

  const handleBecomeCreator = async () => {
    await api.put('/users/me/become-creator');
    const { data } = await api.get('/users/me');
    localStorage.setItem('user', JSON.stringify(data));
    setUser(data);
  };

  if (!user) return null;

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-4xl mx-auto">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-3xl font-bold">Dashboard</h1>
          <Button variant="outline" onClick={handleLogout}>Изход</Button>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <Card>
            <CardHeader>
              <CardTitle>Профил</CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
              <p><span className="font-medium">Име:</span> {user.displayName}</p>
              <p><span className="font-medium">Email:</span> {user.email}</p>
              <p><span className="font-medium">Роля:</span> {user.role}</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Бързи действия</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              {user.role === 'Buyer' && (
                <Button className="w-full" onClick={handleBecomeCreator}>
                  Стани Creator
                </Button>
              )}
              <Button className="w-full" variant="outline" onClick={() => router.push('/products')}>
                Разгледай продукти
              </Button>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}