import React from 'react';
import Sidebar from './Sidebar';

const MainLayout = ({ children }) => (
    <div className="h-screen flex bg-gray-100">
        <Sidebar />
        <main className="flex-1 p-6 lg:p-10 overflow-y-auto">
            {children}
        </main>
    </div>
);

export default MainLayout;
