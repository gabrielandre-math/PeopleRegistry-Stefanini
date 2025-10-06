import React, { useState, useEffect } from 'react';
import { useApiVersion } from '../contexts/apiVersionProvider.jsx';
import Input from '../components/ui/Input.jsx';
import Select from '../components/ui/Select.jsx';
import Button from '../components/ui/Button.jsx';

const emptyAddress = { street: '', number: '', city: '', state: '', zipCode: '' };

const mkInitial = () => ({
  name: '',
  cpf: '',
  dateOfBirth: '',
  email: '',
  gender: '',          
  placeOfBirth: '',
  nationality: '',
  address: { ...emptyAddress },
});

const PersonForm = ({ person, onFormSubmit, onCancel }) => {
  const [formData, setFormData] = useState(mkInitial());
  const [errors, setErrors] = useState({});
  const [apiVersion] = useApiVersion();

  useEffect(() => {
    const next = mkInitial();

    if (person) {
      next.name = person.name ?? '';
      next.cpf = person.cpf ?? '';
      next.dateOfBirth = person.dateOfBirth ? person.dateOfBirth.split('T')[0] : '';
      next.email = person.email ?? '';
      next.gender = person.gender ?? '';
      next.placeOfBirth = person.placeOfBirth ?? '';
      next.nationality = person.nationality ?? '';
      next.address = { ...emptyAddress, ...(person.address || {}) };
    }

    if (!next.address) next.address = { ...emptyAddress };

    setFormData(next);
  }, [person, apiVersion]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name.startsWith('address.')) {
      const field = name.split('.')[1];
      setFormData((prev) => ({
        ...prev,
        address: { ...(prev.address || emptyAddress), [field]: value },
      }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const validate = () => {
    const newErrors = {};
    if (!formData.name) newErrors.name = 'Nome é obrigatório.';
    if (!formData.cpf) newErrors.cpf = 'CPF é obrigatório.';
    else if (!/^\d{11}$/.test(String(formData.cpf).replace(/\D/g, '')))
      newErrors.cpf = 'CPF deve ter 11 dígitos.';
    if (!formData.dateOfBirth) newErrors.dateOfBirth = 'Data de Nascimento é obrigatória.';

    if (apiVersion === 'v2') {
      if (!formData.address.street) newErrors.street = 'Rua é obrigatória.';
      if (!formData.address.city) newErrors.city = 'Cidade é obrigatória.';
      if (!formData.address.state) newErrors.state = 'Estado é obrigatório.';
      if (!formData.address.zipCode) newErrors.zipCode = 'CEP é obrigatório.';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validate()) return;

    const dataToSubmit = {
      name: formData.name,
      cpf: String(formData.cpf).replace(/\D/g, ''),
      dateOfBirth: formData.dateOfBirth,
      email: formData.email,
      gender: formData.gender === '' ? null : Number(formData.gender),
      placeOfBirth: formData.placeOfBirth,
      nationality: formData.nationality,
    };
    if (apiVersion === 'v2') dataToSubmit.address = { ...formData.address };

    onFormSubmit(dataToSubmit);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <div>
        <h4 className="text-sm font-semibold text-gray-700 mb-3">Dados pessoais</h4>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Input id="name" name="name" label="Nome completo"
                 placeholder="Ex.: Maria da Silva"
                 value={formData.name ?? ''} onChange={handleChange} error={errors.name}/>
          <Input id="cpf" name="cpf" label="CPF" placeholder="Somente números"
                 value={formData.cpf ?? ''} onChange={handleChange} error={errors.cpf}/>
          <Input id="dateOfBirth" name="dateOfBirth" label="Data de nascimento" type="date"
                 value={formData.dateOfBirth ?? ''} onChange={handleChange} error={errors.dateOfBirth}/>
          <Input id="email" name="email" label="Email" type="email" placeholder="email@empresa.com"
                 value={formData.email ?? ''} onChange={handleChange}/>
          <Select id="gender" name="gender" label="Gênero"
                  value={formData.gender ?? ''} onChange={handleChange}>
            <option value="">— selecione —</option>
            <option value="0">Masculino</option>
            <option value="1">Feminino</option>
            <option value="2">Outro</option>
          </Select>
          <Input id="placeOfBirth" name="placeOfBirth" label="Naturalidade" placeholder="Cidade/UF"
                 value={formData.placeOfBirth ?? ''} onChange={handleChange}/>
          <Input id="nationality" name="nationality" label="Nacionalidade" placeholder="Ex.: Brasileira"
                 value={formData.nationality ?? ''} onChange={handleChange}/>
        </div>
      </div>

      {apiVersion === 'v2' && (
        <div>
          <h4 className="text-sm font-semibold text-gray-700 mb-3">Endereço</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input id="address.street" name="address.street" label="Rua"
                   value={formData.address?.street ?? ''} onChange={handleChange} error={errors.street}/>
            <Input id="address.number" name="address.number" label="Número"
                   value={formData.address?.number ?? ''} onChange={handleChange}/>
            <Input id="address.city" name="address.city" label="Cidade"
                   value={formData.address?.city ?? ''} onChange={handleChange} error={errors.city}/>
            <Input id="address.state" name="address.state" label="Estado"
                   value={formData.address?.state ?? ''} onChange={handleChange} error={errors.state}/>
            <Input id="address.zipCode" name="address.zipCode" label="CEP" placeholder="Somente números"
                   value={formData.address?.zipCode ?? ''} onChange={handleChange} error={errors.zipCode}/>
          </div>
        </div>
      )}

      <div className="flex justify-end gap-4 pt-2">
        <Button type="button" variant="secondary" onClick={onCancel}>Cancelar</Button>
        <Button type="submit" variant="primary">Salvar Pessoa</Button>
      </div>
    </form>
  );
};

export default PersonForm;
