import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

import { ToastProvider } from './contexts/toastProvider.jsx';
import { ApiVersionProvider } from './contexts/apiVersionProvider.jsx';
import ProtectedRoute from './ProtectedRoute.jsx';
import LoginPage from './pages/LoginPage.jsx';
import DashboardPage from './pages/DashboardPage.jsx';
import MainLayout from './components/layout/MainLayout.jsx'; 

const queryClient = new QueryClient();

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ToastProvider>
        <ApiVersionProvider>
          <BrowserRouter>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route element={<ProtectedRoute />}>
            
                <Route
                  path="/"
                  element={
                    <MainLayout>
                      <DashboardPage />
                    </MainLayout>
                  }
                />
              </Route>
              <Route path="*" element={<Navigate to="/" />} />
            </Routes>
          </BrowserRouter>
        </ApiVersionProvider>
      </ToastProvider>
    </QueryClientProvider>
  );
}
