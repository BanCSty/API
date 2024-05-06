﻿# Схема баззы данных
![image](https://github.com/BanCSty/WebApplication/assets/67982113/6e31662e-f30b-492b-b468-97b3e6d207f5)

## Таблица Founder(Учредитель)
Созедржит данные об учредителе:
- Id
- Имя
- Фамилия
- Отчество
- ИНН
- Дата создания записи
- Дата последнего обновления записи

## Таблица LegalEntity(Юридическое лицо)
Созедржит данные о юридическом лице:
- Id
- Название юр лица
- ИНН
- Дата создания записи
- Дата последнего обновления записи

## Таблица IndividualEntrepreneur(Иднивидуальный предприниматель)
Созедржит данные о ИП:
- Id
- Название ИП
- ИНН
- Дата создания записи
- Дата последнего обновления записи

## Таблица LegalEntityFounder 
- Является промежуточной таблицей для связи многие ко многим между [Учредителем] и [Юридическим лицом]

## Связи между таблицами
### Founder - LegalEntity
Из описания задания следует - у Юр лица может быть несколько учредителей:
- отношение "многие ко многим" между Founder и LegalEntity позволяет описать связь,
 где каждый учредитель может быть связан с несколькими юридическими лицами, 
 а каждое юридическое лицо может иметь несколько учредителей

### Founder - IndividualEntrepreneur
- У ИП может быть только один учредитель, следовательно у учредителя может быть только одно ИП.
Из этого следует - связь один к одному.