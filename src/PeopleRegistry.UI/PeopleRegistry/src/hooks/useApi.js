import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import api from '../services/api.js';
import { useAuthStore } from '../store/authStore';

// CREATE
export const useCreatePerson = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ personData, apiVersion }) => {
      const endpoint = apiVersion === 'v1' ? '/v1/people/register' : '/v2/people';
      return api.post(endpoint, personData);
    },
    onSuccess: (_data, variables) => {
      queryClient.invalidateQueries({ queryKey: ['people', variables.apiVersion] });
    },
  });
};

// UPDATE
export const useUpdatePerson = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, personData, apiVersion }) => {
      const endpoint = `/${apiVersion}/people/${id}`;
      return api.put(endpoint, personData);
    },
    onSuccess: (_data, variables) => {
      queryClient.invalidateQueries({ queryKey: ['people', variables.apiVersion] });
      queryClient.invalidateQueries({ queryKey: ['person', variables.id, variables.apiVersion] });
    },
  });
};

// DELETE
export const useDeletePerson = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, apiVersion }) => {
      const endpoint = `/${apiVersion}/people/${id}`;
      return api.delete(endpoint);
    },
    onSuccess: (_data, variables) => {
      queryClient.invalidateQueries({ queryKey: ['people', variables.apiVersion] });
    },
  });
};

// LIST
export const usePeople = (apiVersion) => {
  return useQuery({
    queryKey: ['people', apiVersion],
    queryFn: async () => {
      const endpoint = apiVersion === 'v1' ? '/v1/people/all' : '/v2/people';
      const { data, status } = await api.get(endpoint);

      if (status === 204 || data == null) {
        return { people: [] };
      }

      // Compat: pode vir { people: [...] } (v1) ou [...] (v2)
      const people = Array.isArray(data) ? data : (data.people ?? []);
      return { people };
    },
    enabled: !!apiVersion,
  });
};

// AUTH: LOGIN
export const useLogin = () => {
  const { setTokens } = useAuthStore.getState();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: (credentials) => api.post('/v1/auth/login', credentials),
    onSuccess: (response) => {
      setTokens(response.data.accessToken, response.data.refreshToken);
      navigate('/');
    },
  });
};

// AUTH: LOGOUT
export const useLogout = () => {
  const { logout, refreshToken } = useAuthStore.getState();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: () => {
      if (!refreshToken) return Promise.resolve();
      return api.post('/v1/auth/logout', { refreshToken });
    },
    onSuccess: () => {
      logout();
      navigate('/login');
    },
    onError: () => {
      // Desloga local mesmo se o servidor recusar (token expirado)
      logout();
      navigate('/login');
    },
  });
};
