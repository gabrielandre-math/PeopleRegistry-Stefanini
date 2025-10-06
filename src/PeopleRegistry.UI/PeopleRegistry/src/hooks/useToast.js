import { useContext } from 'react';
import { ToastContext } from '../contexts/toastProvider.jsx';

export const useToast = () => useContext(ToastContext);
