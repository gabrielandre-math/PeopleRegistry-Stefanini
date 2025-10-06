import React, { useState } from 'react';
import { usePeople, useCreatePerson, useUpdatePerson, useDeletePerson } from '../hooks/useApi.js';
import { useApiVersion } from '../contexts/apiVersionProvider.jsx';
import { useToast } from '../hooks/useToast.js';
import Button from '../components/ui/Button.jsx';
import Modal from '../components/ui/Modal.jsx';
import { IconLoader, IconUsers, IconPlus, IconAlertTriangle } from '../components/ui/Icons.jsx';
import PeopleTable from './PeopleTable.jsx';
import PersonForm from './PersonForm.jsx';

const DashboardPage = () => {
  const [apiVersion, setApiVersion] = useApiVersion();
  const { data, isLoading, isError, error } = usePeople(apiVersion);
  const { addToast } = useToast();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingPerson, setEditingPerson] = useState(null);
  const [deletingPerson, setDeletingPerson] = useState(null);
  const [viewingPerson, setViewingPerson] = useState(null); 

  const createPersonMutation = useCreatePerson();
  const updatePersonMutation = useUpdatePerson();
  const deletePersonMutation = useDeletePerson();

  const handleOpenCreateModal = () => {
    setEditingPerson(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (person) => {
    setEditingPerson(person);
    setIsModalOpen(true);
  };

  const handleOpenViewModal = (person) => {
    setViewingPerson(person);
  };

  const handleFormSubmit = (personData) => {
    const mutation = editingPerson ? updatePersonMutation : createPersonMutation;
    const params = editingPerson
      ? { id: editingPerson.id, personData, apiVersion }
      : { personData, apiVersion };

    mutation.mutate(params, {
      onSuccess: () => {
        addToast(`Pessoa ${editingPerson ? 'atualizada' : 'cadastrada'} com sucesso!`);
        setIsModalOpen(false);
      },
      onError: (err) => {
        const errorMessages =
          err.response?.data?.errors?.join(', ') ||
          err.response?.data?.message ||
          err.message;
        addToast(`Erro: ${errorMessages}`, 'error');
      }
    });
  };

  const handleDeleteConfirm = () => {
    if (deletingPerson) {
      deletePersonMutation.mutate(
        { id: deletingPerson.id, apiVersion },
        {
          onSuccess: () => {
            addToast('Pessoa excluída com sucesso!');
            setDeletingPerson(null);
          },
          onError: (err) => {
            addToast(`Erro ao excluir: ${err.response?.data?.message || err.message}`, 'error');
          }
        }
      );
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">Dashboard de Pessoas</h1>
          <p className="text-gray-500 mt-1">Gerencie os registros de pessoas no sistema.</p>
        </div>
        <div className="flex items-center gap-4">
          <div className="flex items-center">
            <label htmlFor="apiVersion" className="mr-2 text-sm font-medium">Versão da API:</label>
            <select
              id="apiVersion"
              value={apiVersion}
              onChange={(e) => setApiVersion(e.target.value)}
              className="rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
            >
              <option value="v1">v1</option>
              <option value="v2">v2</option>
            </select>
          </div>
          <Button onClick={handleOpenCreateModal}>
            <IconPlus className="w-5 h-5 mr-2" />
            Registrar Pessoa
          </Button>
        </div>
      </div>

      {isLoading && (
        <div className="text-center p-8">
          <IconLoader className="w-8 h-8 mx-auto text-gray-400" />
        </div>
      )}

      {isError && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-md">
          Erro ao carregar dados: {error.message}
        </div>
      )}

      {!isLoading && !isError && (
        data?.people.length > 0 ? (
          <PeopleTable
            people={data.people}
            onView={handleOpenViewModal}       
            onEdit={handleOpenEditModal}
            onDelete={setDeletingPerson}
          />
        ) : (
          <div className="text-center p-12 bg-white rounded-lg shadow-md">
            <IconUsers className="w-16 h-16 mx-auto text-gray-300" />
            <h3 className="mt-4 text-xl font-semibold text-gray-700">Nenhum registro encontrado</h3>
            <p className="mt-1 text-gray-500">Comece cadastrando uma nova pessoa.</p>
          </div>
        )
      )}

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingPerson ? 'Editar Pessoa' : 'Registrar Nova Pessoa'}
      >
        <PersonForm
          person={editingPerson}
          onFormSubmit={handleFormSubmit}
          onCancel={() => setIsModalOpen(false)}
        />
      </Modal>

      <Modal
        isOpen={!!viewingPerson}
        onClose={() => setViewingPerson(null)}
        title="Detalhes da Pessoa"
      >
        {viewingPerson && (
          <div className="space-y-2 text-sm">
            <div><span className="font-semibold">Nome:</span> {viewingPerson.name}</div>
            <div><span className="font-semibold">CPF:</span> {viewingPerson.cpf}</div>
            {viewingPerson.email && (
              <div><span className="font-semibold">Email:</span> {viewingPerson.email}</div>
            )}
            {viewingPerson.dateOfBirth && (
              <div>
                <span className="font-semibold">Nascimento:</span>{' '}
                {String(viewingPerson.dateOfBirth).slice(0, 10)}
              </div>
            )}
            {viewingPerson.placeOfBirth && (
              <div><span className="font-semibold">Naturalidade:</span> {viewingPerson.placeOfBirth}</div>
            )}
            {viewingPerson.nationality && (
              <div><span className="font-semibold">Nacionalidade:</span> {viewingPerson.nationality}</div>
            )}

            {viewingPerson.address && (
              <div className="mt-3">
                <div className="font-semibold">Endereço</div>
                <div>{viewingPerson.address.street}, {viewingPerson.address.number}</div>
                <div>{viewingPerson.address.city} - {viewingPerson.address.state}</div>
                <div>CEP: {viewingPerson.address.zipCode}</div>
              </div>
            )}
          </div>
        )}
      </Modal>

      <Modal isOpen={!!deletingPerson} onClose={() => setDeletingPerson(null)} title="Confirmar Exclusão">
        <div className="text-center">
          <IconAlertTriangle className="w-16 h-16 mx-auto text-red-500" />
          <h3 className="mt-4 text-lg font-medium text-gray-900">Você tem certeza?</h3>
          <p className="mt-2 text-sm text-gray-500">
            Deseja realmente excluir <strong>{deletingPerson?.name}</strong>? Esta ação não pode ser desfeita.
          </p>
        </div>
        <div className="mt-6 flex justify-center gap-4">
          <Button variant="secondary" onClick={() => setDeletingPerson(null)}>Cancelar</Button>
          <Button variant="danger" onClick={handleDeleteConfirm}>Excluir</Button>
        </div>
      </Modal>
    </div>
  );
};

export default DashboardPage;
