using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;



namespace CsDeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	public partial class Privacy : Window
	{

		public Privacy(Window w)
		{
			InitializeComponent();

			this.Owner = w;
		}
		#region + properties

		public string WindowTitle
		{
			get => Title;
			set => Title = value;
		}

		public double ParentLeft
		{
			set
			{
				if (value >= 0)
					this.Left = value + 50.0;
			}
		}

		public double ParentTop
		{
			set
			{
				if (value >= 0)
					this.Top = value + 50.0;
			}
		}

		#endregion

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;

			Close();
		}

		private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		public Uri WebSite
		{
			get
			{
				// return new Uri("http://www.cyberstudio.pro/?page_id=595");
				return new Uri("https://www.cyberstudioapps.com/?page_id=595");
			}
		}

		public string policy => 
@"General Privacy Policy

Welcome to http://www.cyberstudio.pro (the “Site”).We understand that privacy online is important to users of our Site, especially when conducting business.This statement governs our privacy policies with respect to those users of the Site (“Visitors”) who visit without transacting business and Visitors who register to transact business on the Site and make use of the various services offered by CyberStudioApps (collectively, “Services”) (“Authorized Customers”).

“Personally Identifiable Information”

refers to any information that identifies or can be used to identify, contact, or locate the person to whom such information pertains, including, but not limited to, name, address, phone number, fax number, email address, financial profiles, social security number, and credit card information. Personally Identifiable Information does not include information that is collected anonymously (that is, without identification of the individual user) or demographic information not connected to an identified individual.

What Personally Identifiable Information is collected?

We may collect basic user profile information from all of our Visitors. We collect the following additional information from our Authorized Customers: the names, addresses, phone numbers and email addresses of Authorized Customers, the nature and size of the business, and the nature and size of the advertising inventory that the Authorized Customer intends to purchase or sell.

What organizations are collecting the information?

In addition to our direct collection of information, our third party service vendors (such as credit card companies, clearinghouses and banks) who may provide such services as credit, insurance, and escrow services may collect this information from our Visitors and Authorized Customers. We do not control how these third parties use such information, but we do ask them to disclose how they use personal information provided to them from Visitors and Authorized Customers. Some of these third parties may be intermediaries that act solely as links in the distribution chain, and do not store, retain, or use the information given to them.

How does the Site use Personally Identifiable Information?

We use Personally Identifiable Information to customize the Site, to make appropriate service offerings, and to fulfill buying and selling requests on the Site. We may email Visitors and Authorized Customers about research or purchase and selling opportunities on the Site or information related to the subject matter of the Site. We may also use Personally Identifiable Information to contact Visitors and Authorized Customers in response to specific inquiries, or to provide requested information.

With whom may the information may be shared?

Personally Identifiable Information about Authorized Customers may be shared with other Authorized Customers who wish to evaluate potential transactions with other Authorized Customers. We may share aggregated information about our Visitors, including the demographics of our Visitors and Authorized Customers, with our affiliated agencies and third party vendors. We also offer the opportunity to “opt out” of receiving information or being contacted by us or by any agency acting on our behalf.

How is Personally Identifiable Information stored?

Personally Identifiable Information collected by CyberStudioApps is securely stored and is not accessible to third parties or employees of CyberStudioApps except for use as indicated above.

What choices are available to Visitors regarding collection, use and distribution of the information?

Visitors and Authorized Customers may opt out of receiving unsolicited information from or being contacted by us and/or our vendors and affiliated agencies by responding to emails as instructed, or by contacting us at 5800 Allington St., Lakewood, CA 90713

Are Cookies Used on the Site?

Cookies are used for a variety of reasons. We use Cookies to obtain information about the preferences of our Visitors and the services they select. We also use Cookies for security purposes to protect our Authorized Customers. For example, if an Authorized Customer is logged on and the site is unused for more than 10 minutes, we will automatically log the Authorized Customer off.

How does CyberStudioApps use login information?

CyberStudioApps uses login information, including, but not limited to, IP addresses, ISPs, and browser types, to analyze trends, administer the Site, track a user’s movement and use, and gather broad demographic information.

What partners or service providers have access to Personally Identifiable Information from Visitors and/or Authorized Customers on the Site?

CyberStudioApps has entered into and will continue to enter into partnerships and other affiliations with a number of vendors.Such vendors may have access to certain Personally Identifiable Information on a need to know basis for evaluating Authorized Customers for service eligibility. Our privacy policy does not cover their collection or use of this information. Disclosure of Personally Identifiable Information to comply with law. We will disclose Personally Identifiable Information in order to comply with a court order or subpoena or a request from a law enforcement agency to release information. We will also disclose Personally Identifiable Information when reasonably necessary to protect the safety of our Visitors and Authorized Customers.

How does the Site keep Personally Identifiable Information secure?

All of our employees are familiar with our security policy and practices. The Personally Identifiable Information of our Visitors and Authorized Customers is only accessible to a limited number of qualified employees who are given a password in order to gain access to the information. We audit our security systems and processes on a regular basis. Sensitive information, such as credit card numbers or social security numbers, is protected by encryption protocols, in place to protect information sent over the Internet. While we take commercially reasonable measures to maintain a secure site, electronic communications and databases are subject to errors, tampering and break-ins, and we cannot guarantee or warrant that such events will not take place and we will not be liable to Visitors or Authorized Customers for any such occurrences.

How can Visitors correct any inaccuracies in Personally Identifiable Information?

Visitors and Authorized Customers may contact us to update Personally Identifiable Information about them or to correct any inaccuracies by emailing us at jeff@cyberstudioapps.com

Can a Visitor delete or deactivate Personally Identifiable Information collected by the Site?

We provide Visitors and Authorized Customers with a mechanism to delete/deactivate Personally Identifiable Information from the Site’s database by contacting . However, because of backups and records of deletions, it may be impossible to delete a Visitor’s entry without retaining some residual information. An individual who requests to have Personally Identifiable Information deactivated will have this information functionally deleted, and we will not sell, transfer, or use Personally Identifiable Information relating to that individual in any way moving forward.

What happens if the Privacy Policy Changes?

We will let our Visitors and Authorized Customers know about changes to our privacy policy by posting such changes on the Site. However, if we are changing our privacy policy in a manner that might cause disclosure of Personally Identifiable Information that a Visitor or Authorized Customer has previously requested not be disclosed, we will contact such Visitor or Authorized Customer to allow such Visitor or Authorized Customer to prevent such disclosure.

Links:

http://www.cyberstudio.pro contains links to other web sites. Please note that when you click on one of these links, you are moving to another web site. We encourage you to read the privacy statements of these linked sites as their privacy policies may differ from ours.

Cookie Privacy Policy
http://www.cyberstudio.pro is committed to protecting your privacy online.

What are Cookies?

Cookies are small files that a site or its service provider transfers to your computers hard drive through your Web browser (if you allow) that enables the sites or service providers systems to recognize your browser and capture and remember certain information We use cookies to understand and save your preferences for future visits.

What information do we collect?

We collect information from you when you visit our site. When submitting or registering on our site, you may be asked to enter your: name or e-mail address. You may, however, visit our site anonymously.

What do we use your information for?

Any of the information we collect from you may be used in one of the following ways: To improve our website and To improve customer service and is not shared with or sold to other organizations for commercial purposes. That being said, your information could be shared under the following circumstances:

We use third parties to facilitate our business, including, but not limited to, sending email and processing payments. In connection with these offerings and business operations, these third parties may have access to your personal information for use in connection with those business activities.

As we develop our business, we may buy or sell assets or business offerings. Customer, email, and visitor information is generally one of the transferred business assets in these types of transactions.

We may also transfer such information in the course of corporate divestitures, mergers, or any dissolution.

If it becomes necessary to share information in order to investigate, prevent, or take action regarding illegal activities, suspected fraud, situations involving potential threats to the physical safety of any person, violations of our Terms of Service, or as otherwise required by law.

How do we protect your information?

We implement a variety of security measures to maintain the safety of your personal information when you place an order or enter, submit, or access your personal information.

Online Cookie Privacy Policy

This online cookie privacy policy applies only to information collected through our website and not to information collected offline.

Changes to our Cookie Privacy Policy

If we decide to change our privacy policy, we will post those changes on this page.

If you have any questions and suggestions regarding our Privacy Policy Statement, please contact us and we will get back to you very soon.

California Privacy Rights
If you are a California resident, California Civil Code Section 1798.83 permits you to request information regarding the disclosure of your personal information by CyberStudioApps to third parties for the third parties’ direct marketing purposes. To make such a request, please send an email to jeff@cyberstudioapps.com or write us: 5800 Allington St., Lakewood, CA 90713 We take great pride in the relationship of trust and we are dedicated to treating your personal information with care and respect. Pursuant to California Civil Code Section 1798.83(c)(2), CyberStudioApps does not share guests’ personal information with other companies or others outside for those parties’ direct marketing use unless a guest elects that we do so.  For more information about our privacy and data collection policies, you may wish to review our Privacy Policy. If you are a California resident under the age of 18, and a registered user of any site where this policy is posted, California Business and Professions Code Section 22581 permits you to request and obtain removal of content or information you have publicly posted. To make such a request, please send an email with a detailed description of the specific content or information to jeff@cyberstudioapps.com. Please be aware that such a request does not ensure complete or comprehensive removal of the content or information you have posted and that there may be circumstances in which the law does not require or allow removal even if requested.

EU Privacy Policy
I. OBJECTIVE

The aim of this EU Privacy Policy (“the Policy”) is to provide adequate and consistent safeguards for the handling of Personal Data (as defined below) by “CyberStudio” in accordance with Directive 95/46/EC of the European Parliament and of the Council of 24 October 1995 on the protection of individuals with regard to the processing of personal data and on the free movement of such data (“the Directive”) and all the relevant transposing legislation of the Directive in the European Union/European Economic Area (“EU/EEA”), the Swiss Federal Data Protection Act, as such laws may from time to time be amended and valid during the application of this Policy, the Privacy Shield (defined below), and any other privacy laws, regulations and principles concerning the collection, storage, use, transfer and other processing of Personal Data transferred from the European Economic Area (“EEA”) or Switzerland to the United States including but not limited to the Regulation (EU) 2016/679 of the European Parliament and of the Council of 27 April 2016 on the protection of natural persons with regard to the processing of personal data and on the free movement of such data, and repealing Directive 95/46/EC (“the General Data Protection Regulation”) as of its entry into force on 24 May 2018.

II. SCOPE

This Policy applies to CyberStudioApps in the EU that process Personal Data.

“Consumer” “Consumer” means any natural person who is located in the EU, but excludes any individual acting in his or her capacity as an Employee.

“Controller” means a person or organization which, alone or jointly with others, determines the purposes and means of the processing of Personal Data as referred to in Privacy Shield materials.

“Employee” means any current, former or prospective employee, temporary worker, intern or other non-permanent employee of CyberStudioApps or any current or prospective subsidiary or affiliate of CyberStudio.

“European Economic Area (“EEA”)” means the following countries: Austria, Belgium, Bulgaria, Cyprus, Czech Republic, Denmark, Estonia, Finland, France, Germany, Greece, Hungary, Iceland, Republic of Ireland, Italy, Latvia, Liechtenstein, Lithuania, Luxembourg, Malta, The Netherlands, Norway, Poland, Portugal, Romania, Slovakia, Slovenia, Spain, Sweden, the UK.

“Personal Data” means any information relating to an identified or identifiable natural person (“data subject”); an identifiable person is one who can be identified, directly or indirectly, in particular by reference to an identification number or to one or more factors specific to his physical, physiological, mental, economic, cultural or social identity and includes information, that (i) relates to an identified or identifiable Customer, Employee or Supplier’s representative; (ii) can be linked to that Customer, Employee or Supplier’s representative; (iii) is transferred to CyberStudioApps in the U.S. from the EEA or Switzerland, and (iv) is recorded in any form.

“Sensitive Personal Data” means Personal Data revealing racial or ethnic origin, political opinions, religious or philosophical beliefs, trade-union membership or concerning health or sex, and the commission or alleged commission of any offense, any proceedings for any offense committed or alleged to have been committed by the individual or the disposal of such proceedings, or the sentence of any court in such proceedings.

“Supplier” means any supplier, vendor or other third party located in the USA and/or the EEA or Switzerland that provides services or products to CyberStudio. For the purposes of this Policy Suppliers shall be included within the definition of “Consumers” above.

“Systems Privacy Point of Contact” means individual officers designated by CyberStudioApps as the initial points of contact for inquiries, complaints, or questions regarding privacy matters. Currently, such officers are identified at the end of this Policy.

“Processing” is defined as any action that is performed on Personal Data, whether in whole or in part by automated means, such as collecting, modifying, using, disclosing, or deleting such data.

This Policy does not cover data rendered anonymous or where pseudonyms are used. Data is rendered anonymous if individuals are no longer identifiable or are identifiable only with a disproportionately large expense in time, cost or labor. The use of pseudonyms involves the replacement of names or other identifiers with substitutes, so that identification of individual persons is either impossible or at least rendered considerably more difficult. If data rendered anonymous become no longer anonymous (i.e. individuals are again identifiable), or if pseudonyms are used and the pseudonyms allow identification of individual persons, then this Policy shall apply again.

III. APPLICATION OF LOCAL LAWS

This Policy is designed to provide compliance with all relevant applicable laws in the EEA and in particular those transposing the Directive. CyberStudioApps recognizes that certain laws might be modified to require stricter standards than those described in this Policy, in which case the stricter standards shall apply. CyberStudioApps will handle Personal Data in accordance with local law at the place where the Personal Data is processed. If applicable law provides for a lower level of protection of Personal Data than that established by this Policy, then this Policy shall prevail.

IV. PRINCIPLES FOR PROCESSING PERSONAL DATA

CyberStudioApps respects Employee, Consumer (including personnel of customers, suppliers, stakeholders, and third parties) privacy and is committed to protecting Personal Data in compliance with the applicable legislation in the EEA. This compliance is consistent with CyberStudio’s desire to keep its Employees and Consumers informed and to recognize and respect their privacy rights. CyberStudioApps will observe the following principles when processing Personal Data:

Data will be processed fairly and in accordance with applicable law.
Data will be collected for specified, legitimate purposes and not processed further in ways incompatible with those purposes.
Data will be relevant to and not excessive for the purposes for which they are collected and used. For example data may be rendered anonymous if deemed reasonable, feasible and appropriate, depending on the nature of the data and the risks associated with the intended uses.
Data subjects in the EU will be asked to provide their clear and unequivocal consent for the collection, processing and transfer of their Personal Data.
Data will be accurate and, where necessary kept up up-to-date. Reasonable steps will be taken to rectify or delete Personal Data that is inaccurate or incomplete.
Data will be kept only as it is necessary for the purposes for which it was collected and processed. Those purposes shall be described in this Policy.
Data will be deleted or amended following a relevant request by the concerned data subject, should such notice comply with the applicable legislation each time.
Data will be processed in accordance with the individual’s legal rights (as described in this Policy or as provided by law).
Appropriate technical, physical and organizational measures will be taken to prevent unauthorized access, unlawful processing and unauthorized or accidental loss, destruction or damage to data. In case of any such violation with respect to Personal Data, CyberStudioApps will take appropriate steps to end the violation and determine liabilities in accordance with applicable law and will cooperate with the competent authorities.

V. TYPES OF DATA PROCESSED

As permitted by local laws, the Personal Data relating to Employees may include the following:

name;
contact information;
date of birth;
government-issued identification information, passport or visa information;
educational history;
employment and military history;
legal work eligibility status;
information about job performance and compensation;
financial account information; and
other information Employees may provide.
Personal Data relating to Consumers may include:

Contact information, such as name, postal address, email address and telephone number; and
Personal Data in content Consumers provide on CyberStudio’s website and other data collected automatically through the website (such as IP addresses, browser characteristics, device characteristics, operating system, language preferences, referring URLs, information on actions taken on our website, and dates and times of website visits).
Financial account information.
CyberStudioApps also may obtain and use Consumer Personal Data in other ways for which CyberStudioApps provides specific notice at the time of collection (including but not limited to e.g. surveys, focus groups, market research, inbound and outbound Consumer communications and education, etc.).

VI. WAYS OF OBTAINING PERSONAL DATA

The ways by which CyberStudioApps obtains Personal Data are defined hereby. CyberStudioApps does not obtain any personal information about Employees or Consumers unless the Employee or Consumer has provided that information to CyberStudioApps in a way providing for its clear and unequivocal consent to do so including but not limited to visiting CyberStudio’s website, by completion of a written employment application, employee benefits application, insurance form, consent form, survey, or completion of an on-line or hard copy form. Employees and Consumers may choose to submit personal, private information by facsimile, regular mail, e-mail, or electronic transmission over our internal web site, interoffice mail, or personal delivery, as each of these methods may be deemed applicable each time.

VII. PURPOSES FOR PERSONAL DATA PROCESSING

CyberStudioApps processes personal data for legitimate purposes related to human resources, business and safety /security. The limitation of purposes shall be taken into consideration before any type of processing of Personal Data and shall not be subject to any changes without prior notification. These principal purposes for Employee Personal Data include:

Human resources purposes including but not limited to recruiting and hiring job applicants, and:

Managing Employee communications and relations
Providing compensation and benefits;
Administering payroll;
Processing corporate expenses and reimbursements;
Managing Employee participation in human resources plans and programs;
Carrying out obligations under employment contracts;
Managing Employee performance;
Conducting training and talent development;
Facilitating Employee relocations and international assignments;
Managing Employee headcount and office allocation;
Managing the Employee termination process;
Managing information technology and communications systems, such as the corporate email system and company directory;
Conducting ethics and disciplinary investigations;
Administering Employee grievances and claims;
Managing audit and compliance matters;
Complying with applicable legal obligations, including government reporting and specific local law requirements; and
Other general human resources purposes.
CyberStudioApps may also obtain and process Personal Data about Employees’ emergency contacts and other individuals (such as spouse, family members, dependents and beneficiaries) to the extent Employees provide such information to CyberStudio. CyberStudioApps processes this information to comply with its legal obligations and for benefits administration and other internal administrative purposes.

For Consumer specific Personal Data, the purposes of processing may include:

Running day-to-day business relationship
Marketing activities
Management of financial accounts
Business Development Activities
Conduct of transactions or facilitation of offering of the CyberStudioApps Services
Conduct of surveys, focus groups, market research, inbound and outbound Consumer communications and education
For Client and Supplier specific information, the purposes of processing may include:

Management of its relationships with its Clients and Suppliers
Processing payments, expenses and reimbursements
Carrying out CyberStudio’s obligations under such contracts
If CyberStudioApps introduces a new process or application that will result in the processing of Personal Data for purposes that go beyond the purposes described above, CyberStudioApps will inform the concerned data subjects of such new process or application, new purpose for which the Personal Data are to be used, and the categories of recipients of the Personal Data.

VIII. SECURITY AND CONFIDENTIALITY

CyberStudioApps is committed to taking appropriate technical, physical and organizational measures to protect Personal Data against unauthorized access, unlawful processing, accidental loss or damage and unauthorized destruction.

Training

CyberStudioApps will be responsible for conducting adequate training sessions regarding the lawful, enumerated intended purposes of processing Personal Data, the need to protect and keep information accurate and up-to-date, the lawful purposes of collecting, handling and processing data that is transferred from the EU to the US and the need to maintain the confidentiality of the data to which employees have access. Authorized users will comply with this Policy and CyberStudioApps will take appropriate actions in accordance with applicable law, if Personal Data are accessed, processed, or used in any way that is inconsistent with the requirements of this Policy.

IX. RIGHTS OF DATA SUBJECTS

Any person has the right to be provided with information as to the nature of the Personal Data stored or processed about him or her by CyberStudioApps and may request deletion or amendments.

All Employees and Consumers have access to their own personal information and may correct or amend it as needed. Employees may view their own personnel record upon request by contacting the local Talent Development contact or by accessing certain information in the company’s internet and/or extranet. Consumers may contact the Privacy POC or Privacy@fleishman.com to review, update, and revise their Personal Data.

If access is denied, the Employee and Consumer has the right to be informed about the reasons for denial. The person affected may resort to the dispute resolution described in Section XIII as well as in any competent regulatory body or authority. CyberStudioApps shall handle in a transparent and timely manner any type of internal dispute resolution procedure about Personal Data is conducted.

If any information is inaccurate or incomplete, the person may request that the data be amended. It is every person’s responsibility to provide Talent Development in the case of Employees, or the Systems Privacy POC in the case of Consumers with accurate Personal Data about him or her and to inform such contacts of any changes. (e.g. new home address or change of name).

If the person demonstrates that the purpose for which the data is being processed in no longer legal or appropriate, the data will be deleted, unless the applicable law requires otherwise.

X. TRANSFERS

Selected Third Parties: CyberStudioApps will not disclose or share any personal information with any external entity or third party, except to an employee’s designated insurance provider, employee benefits administrator, travel professionals, clients to illustrate experience and qualifications for business purposes or promotion and not beyond that, to third party vendors and/or marketers upon Consumer’s explicit consent or as an employee or consumer may designate.
Other Third Parties: CyberStudioApps may be required to disclose certain Personal Data to other third parties: (i) As a matter of law (e.g. to tax and social security authorities); (ii) to protect CyberStudio’s legal rights; (iii) in an emergency where the health or security of an employee is endangered (e.g. a fire); (iv) to Law Enforcement Authorities in accordance with the relevant legislation in the different EEA Member States including but not limited to legislation transposing the EU/2016/1148 concerning measures for a high common level of security of network and information systems across the Union (“the Network Information Security Directive”).

Automated decisions are defined as decisions about individuals that are based solely on the automated processing of data and that produce legal effects that significantly affect the individuals involved.

CyberStudioApps does not make automated decisions for Employee or Consumer data. If automated decisions are made, affected persons will be given an opportunity to express their views on the automated decision in question and object to it.

XII. ENFORCEMENT RIGHTS AND MECHANISMS

CyberStudioApps will ensure that this Policy is observed and duly implemented. All persons who have access to Personal Data must comply with this Policy. Violations of the applicable data protection legislation in the EEA may lead to penalties and/or claims for damages.

If at any time, a person believes that Personal Data relating to him or her has been processed in violation of this Policy, he or she may report the concern to the competent CyberStudio’s official.

XIV. COMMUNICATION ABOUT THE POLICY

In addition to the training on this Policy, CyberStudioApps will communicate this Policy to current and new employees and consumers by posting it on the company’s websites.

MODIFICATIONS OF THE POLICY
CyberStudioApps reserves the right to modify this Policy as needed, for example, to comply with changes in laws, regulations or requirements introduced by DPAs. CyberStudioApps will post all changes to the Policy on its websites.

XVI. OBLIGATIONS TOWARDS DATA PROTECTION AUTHORITIES

CyberStudioApps will respond diligently and appropriately to requests from DPAs about this Policy or compliance with applicable data protection privacy laws and regulations. 

Contact Information:
CyberStudioApps
Mailing address:
5800 Allington St.
Lakewood, CA 90713
United States
Phone: 8186407809
Contact Email: jeff@cyberstudioapps.com
";
	}
}
