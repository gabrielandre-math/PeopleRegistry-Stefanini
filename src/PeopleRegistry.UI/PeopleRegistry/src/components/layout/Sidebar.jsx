import React from 'react';
import { Link } from 'react-router-dom';
import { useAuthStore } from '../../store/authStore';
import { useLogout } from '../../hooks/useApi';
import { IconBuilding, IconUsers, IconLogOut } from '../ui/Icons';

const Sidebar = () => {
  const userEmail = useAuthStore((state) => state.userEmail);
  const logoutMutation = useLogout();

  const handleLogout = () => {
    logoutMutation.mutate();
  };

  return (
    <aside className="w-64 bg-gray-800 text-white flex-col hidden md:flex">
      <div className="h-16 flex items-center justify-center text-xl font-bold border-b border-gray-700">
        <IconBuilding className="w-7 h-7 mr-2" />
        <span>PeopleReg</span>
      </div>

      <nav className="flex-1 px-4 py-6 space-y-2">
        <Link to="/" className="flex items-center px-4 py-2 rounded-md bg-gray-700 text-white">
          <IconUsers className="w-5 h-5 mr-3" />
          Pessoas
        </Link>
      </nav>

      <div className="px-4 py-4 border-t border-gray-700">
        <p className="text-sm text-gray-400 truncate" title={userEmail}>{userEmail}</p>
        <button
          onClick={handleLogout}
          className="w-full flex items-center mt-2 px-4 py-2 rounded-md text-sm text-left text-gray-300 hover:bg-red-600 hover:text-white transition-colors"
        >
          <IconLogOut className="w-5 h-5 mr-3" />
          Sair
        </button>
      </div>
    </aside>
  );
};

export default Sidebar;
