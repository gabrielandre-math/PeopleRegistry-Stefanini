import React from 'react';
import { IconEye, IconEdit, IconTrash2 } from '../components/ui/Icons.jsx';

const PeopleTable = ({ people, onView, onEdit, onDelete }) => (
  <div className="bg-white shadow-md rounded-lg overflow-hidden">
    <table className="min-w-full divide-y divide-gray-200">
      <thead className="bg-gray-50">
        <tr>
          <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome</th>
          <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">CPF</th>
          <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ações</th>
        </tr>
      </thead>
      <tbody className="bg-white divide-y divide-gray-200">
        {people.map(person => (
          <tr key={person.id} className="hover:bg-gray-50">
            <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{person.name}</td>
            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{person.cpf}</td>
            <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-2">
              <button
                onClick={() => onView?.(person)}
                className="text-blue-600 hover:text-blue-800"
                title="Visualizar"
              >
                <IconEye className="w-5 h-5 inline" />
              </button>
              <button
                onClick={() => onEdit(person)}
                className="text-yellow-600 hover:text-yellow-800"
                title="Editar"
              >
                <IconEdit className="w-5 h-5 inline" />
              </button>
              <button
                onClick={() => onDelete(person)}
                className="text-red-600 hover:text-red-800"
                title="Excluir"
              >
                <IconTrash2 className="w-5 h-5 inline" />
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </div>
);

export default PeopleTable;
