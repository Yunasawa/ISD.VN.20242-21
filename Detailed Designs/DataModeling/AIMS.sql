--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Book; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Book" (
    id integer NOT NULL,
    author character varying(45) NOT NULL,
    "coverType" character varying(45) NOT NULL,
    "Publisher" character varying(45) NOT NULL,
    "publishDate" date NOT NULL,
    "numOfPages" integer NOT NULL,
    language character varying(45) NOT NULL,
    "bookCategory" character varying(45) NOT NULL
);


ALTER TABLE public."Book" OWNER TO postgres;

--
-- Name: CD; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."CD" (
    id integer NOT NULL,
    artist character varying(45) NOT NULL,
    "recordLabel" character varying(45) NOT NULL,
    "musicType" character varying(45) NOT NULL,
    "releasedDate" date
);


ALTER TABLE public."CD" OWNER TO postgres;

--
-- Name: Card; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Card" (
    id integer NOT NULL,
    "cardCode" character varying(45) NOT NULL,
    owner character varying(45) NOT NULL,
    "cvvCode" character varying(3) NOT NULL,
    "dateExpired" character varying(4) NOT NULL
);


ALTER TABLE public."Card" OWNER TO postgres;

--
-- Name: Card_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Card_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Card_id_seq" OWNER TO postgres;

--
-- Name: Card_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Card_id_seq" OWNED BY public."Card".id;


--
-- Name: DVD; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DVD" (
    id integer NOT NULL,
    "discType" character varying(45) NOT NULL,
    director character varying(45) NOT NULL,
    runtime integer NOT NULL,
    studio character varying(45) NOT NULL,
    subtitle character varying(45) NOT NULL,
    "releasedDate" date NOT NULL,
    "filmType" character varying(45) NOT NULL
);


ALTER TABLE public."DVD" OWNER TO postgres;

--
-- Name: DeleveryInfo; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DeleveryInfo" (
    id integer NOT NULL,
    name character varying(45) NOT NULL,
    province character varying(45) NOT NULL,
    instructions character varying(200),
    address character varying(100) NOT NULL
);


ALTER TABLE public."DeleveryInfo" OWNER TO postgres;

--
-- Name: DeleveryInfo_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."DeleveryInfo_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."DeleveryInfo_id_seq" OWNER TO postgres;

--
-- Name: DeleveryInfo_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."DeleveryInfo_id_seq" OWNED BY public."DeleveryInfo".id;


--
-- Name: Invoice; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Invoice" (
    id integer NOT NULL,
    "totalAmount" integer NOT NULL,
    "orderId" integer NOT NULL
);


ALTER TABLE public."Invoice" OWNER TO postgres;

--
-- Name: Media; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Media" (
    id integer NOT NULL,
    category character varying(45) NOT NULL,
    price integer NOT NULL,
    quantity integer NOT NULL,
    title character varying(45) NOT NULL,
    value integer NOT NULL,
    "imageUrl" character varying(1000) NOT NULL
);


ALTER TABLE public."Media" OWNER TO postgres;

--
-- Name: Media_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Media_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Media_id_seq" OWNER TO postgres;

--
-- Name: Media_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Media_id_seq" OWNED BY public."Media".id;


--
-- Name: Order; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Order" (
    id integer NOT NULL,
    "shippingFees" character varying(45) NOT NULL,
    "deleveryInfoId" integer NOT NULL
);


ALTER TABLE public."Order" OWNER TO postgres;

--
-- Name: OrderMedia; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OrderMedia" (
    "orderID" integer NOT NULL,
    price integer NOT NULL,
    quantity integer NOT NULL,
    "mediaId" integer NOT NULL
);


ALTER TABLE public."OrderMedia" OWNER TO postgres;

--
-- Name: PaymentTransaction; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."PaymentTransaction" (
    id integer NOT NULL,
    "createAt" date NOT NULL,
    content character varying(45) NOT NULL,
    method character varying(45) NOT NULL,
    "cardId" integer NOT NULL,
    "invoiceId" integer NOT NULL
);


ALTER TABLE public."PaymentTransaction" OWNER TO postgres;

--
-- Name: Role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Role" (
    id integer NOT NULL,
    role_name character varying(50) NOT NULL
);


ALTER TABLE public."Role" OWNER TO postgres;

--
-- Name: Role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Role_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Role_id_seq" OWNER TO postgres;

--
-- Name: Role_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Role_id_seq" OWNED BY public."Role".id;


--
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    id integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(50) NOT NULL,
    email character varying(50) NOT NULL,
    phone character varying(20) NOT NULL,
    created_at date NOT NULL
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- Name: UserRole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserRole" (
    user_id integer NOT NULL,
    role_id integer NOT NULL
);


ALTER TABLE public."UserRole" OWNER TO postgres;

--
-- Name: User_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."User_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."User_id_seq" OWNER TO postgres;

--
-- Name: User_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."User_id_seq" OWNED BY public."User".id;


--
-- Name: Card id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Card" ALTER COLUMN id SET DEFAULT nextval('public."Card_id_seq"'::regclass);


--
-- Name: DeleveryInfo id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DeleveryInfo" ALTER COLUMN id SET DEFAULT nextval('public."DeleveryInfo_id_seq"'::regclass);


--
-- Name: Media id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Media" ALTER COLUMN id SET DEFAULT nextval('public."Media_id_seq"'::regclass);


--
-- Name: Role id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role" ALTER COLUMN id SET DEFAULT nextval('public."Role_id_seq"'::regclass);


--
-- Name: User id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User" ALTER COLUMN id SET DEFAULT nextval('public."User_id_seq"'::regclass);


--
-- Data for Name: Book; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Book" (id, author, "coverType", "Publisher", "publishDate", "numOfPages", language, "bookCategory") FROM stdin;
\.


--
-- Data for Name: CD; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."CD" (id, artist, "recordLabel", "musicType", "releasedDate") FROM stdin;
\.


--
-- Data for Name: Card; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Card" (id, "cardCode", owner, "cvvCode", "dateExpired") FROM stdin;
\.


--
-- Data for Name: DVD; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DVD" (id, "discType", director, runtime, studio, subtitle, "releasedDate", "filmType") FROM stdin;
\.


--
-- Data for Name: DeleveryInfo; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DeleveryInfo" (id, name, province, instructions, address) FROM stdin;
\.


--
-- Data for Name: Invoice; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Invoice" (id, "totalAmount", "orderId") FROM stdin;
\.


--
-- Data for Name: Media; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Media" (id, category, price, quantity, title, value, "imageUrl") FROM stdin;
\.


--
-- Data for Name: Order; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Order" (id, "shippingFees", "deleveryInfoId") FROM stdin;
\.


--
-- Data for Name: OrderMedia; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."OrderMedia" ("orderID", price, quantity, "mediaId") FROM stdin;
\.


--
-- Data for Name: PaymentTransaction; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."PaymentTransaction" (id, "createAt", content, method, "cardId", "invoiceId") FROM stdin;
\.


--
-- Data for Name: Role; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Role" (id, role_name) FROM stdin;
\.


--
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."User" (id, username, password, email, phone, created_at) FROM stdin;
\.


--
-- Data for Name: UserRole; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserRole" (user_id, role_id) FROM stdin;
\.


--
-- Name: Card_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Card_id_seq"', 1, false);


--
-- Name: DeleveryInfo_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."DeleveryInfo_id_seq"', 1, false);


--
-- Name: Media_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Media_id_seq"', 2, true);


--
-- Name: Role_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Role_id_seq"', 1, false);


--
-- Name: User_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_id_seq"', 1, false);


--
-- Name: Book Book_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "Book_pkey" PRIMARY KEY (id);


--
-- Name: CD CD_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CD"
    ADD CONSTRAINT "CD_pkey" PRIMARY KEY (id);


--
-- Name: Card Card_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Card"
    ADD CONSTRAINT "Card_pkey" PRIMARY KEY (id);


--
-- Name: DVD DVD_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DVD"
    ADD CONSTRAINT "DVD_pkey" PRIMARY KEY (id);


--
-- Name: DeleveryInfo DeleveryInfo_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DeleveryInfo"
    ADD CONSTRAINT "DeleveryInfo_pkey" PRIMARY KEY (id);


--
-- Name: Invoice Invoice_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Invoice"
    ADD CONSTRAINT "Invoice_pkey" PRIMARY KEY (id);


--
-- Name: Media Media_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Media"
    ADD CONSTRAINT "Media_pkey" PRIMARY KEY (id);


--
-- Name: OrderMedia OrderMedia_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrderMedia"
    ADD CONSTRAINT "OrderMedia_pkey" PRIMARY KEY ("orderID", "mediaId");


--
-- Name: Order Order_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT "Order_pkey" PRIMARY KEY (id, "deleveryInfoId");


--
-- Name: PaymentTransaction PaymentTransaction_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentTransaction"
    ADD CONSTRAINT "PaymentTransaction_pkey" PRIMARY KEY (id, "cardId", "invoiceId");


--
-- Name: Role Role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY (id);


--
-- Name: UserRole UserRole_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "UserRole_pkey" PRIMARY KEY (user_id, role_id);


--
-- Name: User User_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (id);


--
-- Name: Invoice_fk_Invoice_Order1_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "Invoice_fk_Invoice_Order1_idx" ON public."Invoice" USING btree ("orderId");


--
-- Name: OrderMedia_fk_OrderMedia_Media1_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "OrderMedia_fk_OrderMedia_Media1_idx" ON public."OrderMedia" USING btree ("mediaId");


--
-- Name: OrderMedia_fk_ordermedia_order_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "OrderMedia_fk_ordermedia_order_idx" ON public."OrderMedia" USING btree ("orderID");


--
-- Name: Order_fk_Order_DeleveryInfo1_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "Order_fk_Order_DeleveryInfo1_idx" ON public."Order" USING btree ("deleveryInfoId");


--
-- Name: PaymentTransaction_fk_PaymentTransaction_Card1_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "PaymentTransaction_fk_PaymentTransaction_Card1_idx" ON public."PaymentTransaction" USING btree ("cardId");


--
-- Name: PaymentTransaction_fk_PaymentTransaction_Invoice1_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "PaymentTransaction_fk_PaymentTransaction_Invoice1_idx" ON public."PaymentTransaction" USING btree ("invoiceId");


--
-- Name: Book fk_Book_Media1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "fk_Book_Media1" FOREIGN KEY (id) REFERENCES public."Media"(id);


--
-- Name: CD fk_CD_Media1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CD"
    ADD CONSTRAINT "fk_CD_Media1" FOREIGN KEY (id) REFERENCES public."Media"(id);


--
-- Name: DVD fk_DVD_Media1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DVD"
    ADD CONSTRAINT "fk_DVD_Media1" FOREIGN KEY (id) REFERENCES public."Media"(id);


--
-- Name: Invoice fk_Invoice_Order1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Invoice"
    ADD CONSTRAINT "fk_Invoice_Order1" FOREIGN KEY ("orderId", "orderId") REFERENCES public."Order"(id, "deleveryInfoId");


--
-- Name: OrderMedia fk_OrderMedia_Media1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrderMedia"
    ADD CONSTRAINT "fk_OrderMedia_Media1" FOREIGN KEY ("mediaId") REFERENCES public."Media"(id);


--
-- Name: Order fk_Order_DeleveryInfo1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT "fk_Order_DeleveryInfo1" FOREIGN KEY ("deleveryInfoId") REFERENCES public."DeleveryInfo"(id);


--
-- Name: PaymentTransaction fk_PaymentTransaction_Card1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentTransaction"
    ADD CONSTRAINT "fk_PaymentTransaction_Card1" FOREIGN KEY ("cardId") REFERENCES public."Card"(id);


--
-- Name: PaymentTransaction fk_PaymentTransaction_Invoice1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentTransaction"
    ADD CONSTRAINT "fk_PaymentTransaction_Invoice1" FOREIGN KEY ("invoiceId") REFERENCES public."Invoice"(id);


--
-- Name: UserRole fk_UserRole_Role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "fk_UserRole_Role" FOREIGN KEY (role_id) REFERENCES public."Role"(id);


--
-- Name: UserRole fk_UserRole_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "fk_UserRole_User" FOREIGN KEY (user_id) REFERENCES public."User"(id);


--
-- Name: OrderMedia fk_ordermedia_order; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrderMedia"
    ADD CONSTRAINT fk_ordermedia_order FOREIGN KEY ("orderID", "orderID") REFERENCES public."Order"(id, "deleveryInfoId");


--
-- PostgreSQL database dump complete
--

