import { types } from 'mobx-state-tree';

export interface WeekOverWeekType {
  id: string;
  relevancy: number;
  dayOfWeek: string;
  value: number;
}

export const WeekOverWeekModel = types
  .model({
    id: types.identifier(types.string),
    relevancy: types.number,
    dayOfWeek: types.string,
    value: types.number
  });

export interface ChartType {
  id: string;
  relevancy: number;
  label: string;
  value: number;
}

export const ChartModel = types
  .model({
    id: types.identifier(types.string),
    relevancy: types.number,
    label: types.string,
    value: types.number
  });

export interface ByStateType {
  id: string;
  relevancy: number;
  state: string;
  value: number;
}

export const ByStateModel = types
  .model({
    id: types.identifier(types.string),
    relevancy: types.number,
    state: types.string,
    value: types.number
  });
