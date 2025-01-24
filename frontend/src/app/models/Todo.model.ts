export interface TodoModel {
  id: string,
  content: string,
  todolistId: string | undefined,
  date: number | undefined
}
