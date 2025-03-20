// This file is auto-generated by @hey-api/openapi-ts

export type BalanceDto = {
    date?: string;
    dailyBalance?: number;
    accumulatedBalance?: number;
};

export type CreateExpenseCommand = {
    type?: string;
    date?: string;
    description?: string;
    value?: number;
};

export type Expense = {
    type?: string;
    date?: string;
    description?: string;
    value?: number;
    id?: string;
    createdAt?: string;
    updatedAt?: string | null;
};

export type PagedResultOfExpense = {
    items?: Array<Expense>;
    pageNumber?: number;
    pageSize?: number;
    totalPages?: number;
    totalItems?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
};

export type UpdateExpenseCommand = {
    id?: string;
    date?: string;
    description?: string;
    value?: number;
};

export type GetAllExpensesData = {
    body?: never;
    path?: never;
    query?: {
        pageSize?: number;
        pageNumber?: number;
    };
    url: '/expenses';
};

export type GetAllExpensesResponses = {
    /**
     * OK
     */
    200: PagedResultOfExpense;
};

export type GetAllExpensesResponse = GetAllExpensesResponses[keyof GetAllExpensesResponses];

export type CreateExpenseData = {
    body: CreateExpenseCommand;
    path?: never;
    query?: never;
    url: '/expenses';
};

export type CreateExpenseResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type DeleteExpenseData = {
    body?: never;
    path: {
        id: string;
    };
    query?: never;
    url: '/expenses/{id}';
};

export type DeleteExpenseErrors = {
    /**
     * Not Found
     */
    404: unknown;
};

export type DeleteExpenseResponses = {
    /**
     * No Content
     */
    204: void;
};

export type DeleteExpenseResponse = DeleteExpenseResponses[keyof DeleteExpenseResponses];

export type GetExpenseByIdData = {
    body?: never;
    path: {
        id: string;
    };
    query?: never;
    url: '/expenses/{id}';
};

export type GetExpenseByIdErrors = {
    /**
     * Not Found
     */
    404: unknown;
};

export type GetExpenseByIdResponses = {
    /**
     * OK
     */
    200: Expense;
};

export type GetExpenseByIdResponse = GetExpenseByIdResponses[keyof GetExpenseByIdResponses];

export type UpdateExpenseData = {
    body: UpdateExpenseCommand;
    path: {
        id: string;
    };
    query?: never;
    url: '/expenses/{id}';
};

export type UpdateExpenseErrors = {
    /**
     * Not Found
     */
    404: unknown;
};

export type UpdateExpenseResponses = {
    /**
     * OK
     */
    200: Expense;
};

export type UpdateExpenseResponse = UpdateExpenseResponses[keyof UpdateExpenseResponses];

export type GetExpenseBalanceData = {
    body?: never;
    path?: never;
    query: {
        startDate: string;
        endDate: string;
    };
    url: '/expenses/balance';
};

export type GetExpenseBalanceResponses = {
    /**
     * OK
     */
    200: Array<BalanceDto>;
};

export type GetExpenseBalanceResponse = GetExpenseBalanceResponses[keyof GetExpenseBalanceResponses];

export type ClientOptions = {
    baseUrl: 'http://localhost:5119' | (string & {});
};