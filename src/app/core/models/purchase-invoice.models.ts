export interface PurchaseInvoiceDto {
  cardCode: string;
  docDate: Date;
  docCurrency: string;
  comments?: string;
  documentLines: PurchaseInvoiceLineDto[];
}

export interface PurchaseInvoiceLineDto {
  itemCode: string;
  quantity: number;
  price: number;
}