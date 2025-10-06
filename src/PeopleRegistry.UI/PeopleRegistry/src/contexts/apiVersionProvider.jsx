import React, { createContext, useState, useMemo, useContext } from 'react';

const ApiVersionContext = createContext(null);

export const ApiVersionProvider = ({ children }) => {
  const [apiVersion, setApiVersion] = useState('v1');
  const value = useMemo(() => [apiVersion, setApiVersion], [apiVersion]);
  return (
    <ApiVersionContext.Provider value={value}>
      {children}
    </ApiVersionContext.Provider>
  );
};

export const useApiVersion = () => {
  const context = useContext(ApiVersionContext);
  if (!context) {
    throw new Error('useApiVersion deve ser usado dentro de um ApiVersionProvider');
  }
  return context; // [apiVersion, setApiVersion]
};
