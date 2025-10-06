import { create } from 'zustand';
import { persist } from 'zustand/middleware';

const getPayloadFromToken = (token) => {
    if (!token) return null;
    try {
        const payload = token.split('.')[1];
        return JSON.parse(atob(payload));
    } catch (e) {
        console.error("Erro ao decodificar token:", e);
        return null;
    }
};

export const useAuthStore = create(
    persist(
        (set) => ({
            accessToken: null,
            refreshToken: null,
            userId: null,
            userEmail: null,
            isAuthenticated: false,
            
            setTokens: (accessToken, refreshToken) => {
                const payload = getPayloadFromToken(accessToken);
                set({
                    accessToken,
                    refreshToken,
                    userId: payload?.sid, 
                    userEmail: payload?.email,
                    isAuthenticated: true,
                });
            },
            
            logout: () => {
                set({
                    accessToken: null,
                    refreshToken: null,
                    userId: null,
                    userEmail: null,
                    isAuthenticated: false,
                });
            },
        }),
        {
            name: 'people-registry-auth', 
        }
    )
);
