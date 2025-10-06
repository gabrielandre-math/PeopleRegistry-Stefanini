import React, { useState } from 'react';
import { IconBuilding, IconLoader } from '../components/ui/Icons.jsx';
import Input from '../components/ui/Input.jsx';
import Button from '../components/ui/Button.jsx';
import { useLogin } from '../hooks/useApi';

const LoginPage = () => {
  const [email, setEmail] = useState('admin@company.com');
  const [password, setPassword] = useState('Admin@123');
  const loginMutation = useLogin();

  const handleSubmit = (e) => {
    e.preventDefault();
    loginMutation.mutate({ email, password });
  };

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center p-4">
      <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8 space-y-8">
        <div className="text-center">
          <IconBuilding className="w-12 h-12 mx-auto text-blue-600" />
          <h2 className="mt-4 text-3xl font-extrabold text-gray-900">Acesse sua conta</h2>
          <p className="mt-2 text-sm text-gray-600">Bem-vindo ao People Registry</p>
        </div>

        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          <div className="rounded-md shadow-sm space-y-4">
            <Input
              id="email"
              label="Email"
              type="email"
              placeholder="seu@email.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              error={loginMutation.isError ? 'Credenciais inválidas' : ''}
            />
            <Input
              id="password"
              label="Senha"
              type="password"
              placeholder="Sua senha"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          <div>
            <Button type="submit" disabled={loginMutation.isPending} className="w-full">
              {loginMutation.isPending ? <IconLoader className="w-5 h-5" /> : 'Entrar'}
            </Button>
          </div>

          {loginMutation.isError && (
            <p className="text-center text-sm text-red-600">
              {loginMutation.error?.response?.data?.message || 'Ocorreu um erro no login.'}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
