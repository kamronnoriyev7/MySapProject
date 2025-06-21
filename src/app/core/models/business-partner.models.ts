export interface BusinessPartnerDto {
  cardCode: string;
  cardName: string;
  cardType: string;
}

export interface BusinessPartnersGetDto {
  cardCode?: string;
  cardName?: string;
  cardType?: string;
  groupCode: number;
  phone1?: string;
  contactPerson?: string;
  creditLimit: number;
}